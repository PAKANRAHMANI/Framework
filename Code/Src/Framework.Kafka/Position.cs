using System;

namespace Framework.Kafka
{
    public readonly struct Position
    {
        public static readonly Position Start = new Position(0, 0);

        public static readonly Position End = new Position(-1, -1);

        public readonly long CommitPosition;

        public readonly long PreparePosition;

        public Position(long commitPosition, long preparePosition)
        {
            if (commitPosition < preparePosition)
                throw new ArgumentException("The commit position cannot be less than the prepare position", "commitPosition");

            CommitPosition = commitPosition;
            PreparePosition = preparePosition;
        }

        public static bool operator <(Position p1, Position p2)
        {
            return p1.CommitPosition < p2.CommitPosition || (p1.CommitPosition == p2.CommitPosition && p1.PreparePosition < p2.PreparePosition);
        }

        public static bool operator >(Position p1, Position p2)
        {
            return p1.CommitPosition > p2.CommitPosition || (p1.CommitPosition == p2.CommitPosition && p1.PreparePosition > p2.PreparePosition);
        }

        public static bool operator >=(Position p1, Position p2)
        {
            return p1 > p2 || p1 == p2;
        }

        public static bool operator <=(Position p1, Position p2)
        {
            return p1 < p2 || p1 == p2;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.CommitPosition == p2.CommitPosition && p1.PreparePosition == p2.PreparePosition;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }
        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position)obj);
        }

        public bool Equals(Position other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CommitPosition.GetHashCode() * 397) ^ PreparePosition.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{CommitPosition}/{PreparePosition}";
        }

    }
}