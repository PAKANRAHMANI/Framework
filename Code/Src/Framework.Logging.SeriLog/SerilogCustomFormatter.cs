using System;
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Framework.Logging.SeriLog;

public class SerilogCustomFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        output.Write("{");

        // Timestamp
        output.Write("\"@timestamp\":\"");
        output.Write(logEvent.Timestamp.ToString("o"));
        output.Write("\",");

        output.Write("\"@date\":\"");
        output.Write(DateOnly.FromDateTime(logEvent.Timestamp.LocalDateTime).ToString("o"));
        output.Write("\",");

        output.Write("\"@time\":\"");
        output.Write(logEvent.Timestamp.LocalDateTime.TimeOfDay.ToString());
        output.Write("\",");

        // Level
        output.Write("\"level\":\"");
        output.Write(logEvent.Level.ToString());
        output.Write("\",");

        // Properties (fields)
        output.Write("\"fields\":{");
        var first = true;
        foreach (var property in logEvent.Properties)
        {
            if (!first)
            {
                output.Write(",");
            }
            first = false;
            JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
            output.Write(":");
            property.Value.Render(output);
        }

        // MessageTemplate and Message are omitted here

        output.Write("}}\n");
    }
}