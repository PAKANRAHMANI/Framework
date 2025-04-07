# Framework Project / پروژه فریمورک

[English](#english) | [فارسی](#فارسی)

## English

### Overview
This is a .NET-based framework project that includes a comprehensive build system and Azure pipeline integration. The project is structured to provide a robust development environment with automated builds and testing capabilities.

### Project Structure
```
├── BuildScript/         # Build automation scripts
├── Code/               # Main source code
│   ├── Src/           # Source files
│   └── Test/          # Test files
├── .github/           # GitHub specific configurations
└── build scripts      # Various build scripts for different platforms
```

### Prerequisites
- .NET SDK (version specified in global.json)
- PowerShell (for Windows builds)
- Git

### Building the Project
You can build the project using any of the following methods:

1. Using PowerShell (Windows):
```powershell
.\build.ps1
```

2. Using Command Prompt (Windows):
```cmd
build.cmd
```

3. Using Shell Script (Unix/Linux):
```bash
./build.sh
```

### Azure Pipeline Integration
The project includes Azure Pipeline configuration (`Framework-Azure-Pipeline.yaml`) for continuous integration and deployment.

### Development
- The main source code is located in the `Code/Src` directory
- Tests are located in the `Code/Test` directory
- Build configurations are managed through the BuildScript directory

---

## فارسی

### معرفی
این یک پروژه فریمورک مبتنی بر .NET است که شامل سیستم ساخت جامع و یکپارچه‌سازی با Azure Pipeline می‌باشد. پروژه به گونه‌ای ساختار یافته است که یک محیط توسعه قوی با قابلیت‌های ساخت و تست خودکار را فراهم می‌کند.

### ساختار پروژه
```
├── BuildScript/         # اسکریپت‌های اتوماسیون ساخت
├── Code/               # کد اصلی
│   ├── Src/           # فایل‌های منبع
│   └── Test/          # فایل‌های تست
├── .github/           # تنظیمات مخصوص GitHub
└── build scripts      # اسکریپت‌های ساخت برای پلتفرم‌های مختلف
```

### پیش‌نیازها
- .NET SDK (نسخه مشخص شده در global.json)
- PowerShell (برای ساخت در ویندوز)
- Git

### ساخت پروژه
می‌توانید پروژه را با استفاده از هر یک از روش‌های زیر بسازید:

1. استفاده از PowerShell (ویندوز):
```powershell
.\build.ps1
```

2. استفاده از Command Prompt (ویندوز):
```cmd
build.cmd
```

3. استفاده از Shell Script (یونیکس/لینوکس):
```bash
./build.sh
```

### یکپارچه‌سازی با Azure Pipeline
پروژه شامل پیکربندی Azure Pipeline (`Framework-Azure-Pipeline.yaml`) برای یکپارچه‌سازی و استقرار مداوم است.

### توسعه
- کد اصلی در پوشه `Code/Src` قرار دارد
- تست‌ها در پوشه `Code/Test` قرار دارند
- تنظیمات ساخت از طریق پوشه BuildScript مدیریت می‌شوند 