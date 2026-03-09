# 🦇 Hero Compliance System

## *"Because Not All Heroes Are Created Equal... And Some Are Legally Problematic"*

![Hero Compliance System Screenshot](./assets/frontpage.png)

---

## 🚨 The Problem

Your corporation has a hero roster. Everything seems fine. Superman is great for PR. Wonder Woman brings excellent leadership. The Flash? Fast delivery times. 

But then... **Batman**.

Look, we're not saying he's not effective. We're just saying that having a vigilante billionaire with no actual superpowers, unlimited resources, and a *very specific* code of justice on your payroll is... **a compliance nightmare**.

---

## 💡 The Solution

The **Hero Compliance System** is a cutting-edge CSV analysis tool that scans your hero roster for the most dangerous liability of them all: **Bruce Wayne, a.k.a. Batman**.

### Features:
- 🔍 **Advanced Batman Detection** - Our proprietary algorithm can identify Batman faster than Superman can hear trouble
- 📊 **CSV Analysis** - Upload your hero roster and get instant compliance reports
- ⚡ **Real-time Results** - Know immediately if you're harboring Gotham's Dark Knight
- 🌙 **Dark Mode** - Because of course it's in dark mode. Have you *seen* Batman's aesthetic?
- ✅ **Compliance Reports** - Get detailed results with timestamps for your legal team

---

## 🏗️ Architecture

Built with the finest superhero-grade technology:

- **Frontend**: Blazor Server (because real-time SignalR feels like the Bat-Signal)
- **Backend**: .NET 10 with Clean Architecture (cleaner than the Batcave, probably)
- **Pattern**: CQRS with MediatR (commands as secure as Batman's identity... or so we thought)
- **UI**: MudBlazor in glorious dark mode (Alfred would approve)
- **Data Processing**: CSV parsing that would make Oracle envious

### Project Structure:
```
Company.App/
├── Company.App.Domain/          # The core truth: Batman = bad for business
├── Company.App.Application/     # Use cases (detecting vigilantes since 2024)
├── Company.App.Infrastructure/  # CSV scanning technology
└── Company.App.Web/            # The Blazor interface (no bats allowed)
```

---

## 🚀 Getting Started

### Prerequisites:
- .NET 10 SDK
- A CSV file with heroes
- A healthy respect for corporate liability

### Run the App:
1. Clone this repository
2. Open the solution in Visual Studio
3. Press F5 (or start `Company.App.Web`)
4. Navigate to `https://localhost:7019/`
5. Upload your hero CSV file
6. Hope you don't see any Gotham-related compliance violations

### CSV Format:
```csv
Name,Identity
Superman,Clark Kent
Wonder Woman,Diana Prince
Batman,Bruce Wayne          👈 ⚠️ THIS WILL TRIGGER AN ALERT
```

---

## 📋 How It Works

1. **Upload**: Select your hero roster CSV file
2. **Scan**: Our advanced scanner reads every hero entry
3. **Detect**: Using the `IsBatmanSpec` (yes, it's a real class), we check for:
   - Name contains "Batman" 
   - Identity contains "Bruce Wayne"
4. **Report**: Get instant feedback:
   - ✅ **Clean Roster** - "No Batman detected. Roster approved."
   - ❌ **Violation Found** - "ALERT: Batman detected in the roster!"

---

## 🎭 Why Batman Specifically?

Great question! While Superman can level a city, and Hulk can literally destroy the planet, at least they:
- Have actual powers (insurance can categorize them)
- Don't operate primarily at night in one city (geographic risk assessment)
- Aren't billionaire CEOs with board meeting conflicts (HR nightmare)
- Don't have a "no guns" rule while throwing batarangs (policy inconsistency)

Batman is just... *complicated*.

---

## 🧪 Testing

We've included two sample CSV files for your convenience:

- `heroes_without_batman.csv` - Safe, compliant, boring
- `heroes_with_batman.csv` - **⚠️ CONTAINS BATMAN** - Use for testing alert systems

---

## 📖 API Documentation

### Endpoint: `POST /api/v1/hero/detectBatman`

**Request**: Multipart form-data with CSV file

**Response**:
```json
{
  "value": {
    "found": true,
    "message": "ALERT: Batman detected in the roster!",
    "checkedAt": "2024-01-15T23:47:00Z"
  },
  "isSuccess": true,
  "message": null
}
```

---

## ⚖️ Legal Disclaimer

This system is provided as-is for entertainment and educational purposes. We are not responsible for:
- Actual Batman finding out about this system
- Bat-related retaliation
- Sudden loss of funding from Wayne Enterprises
- Mysterious equipment failures during demos
- Your server room being filled with bats

*If Batman appears at your office, remain calm and explain you were "just following company policy."*

---

## 🤝 Contributing

Pull requests welcome! Unless you're Batman. Then please see HR first.

---

## 📝 License

MIT License - Because even Batman respects open source

---

## 🎯 Future Features

- [ ] Joker detection (for balance)
- [ ] Robin counter (how many sidekicks is too many?)
- [ ] Batmobile parking violation tracker
- [ ] Alfred background check integration
- [ ] Wayne Enterprises stock correlation analysis

---

## 👨‍💻 Author

Built by developers who definitely aren't Batman. We have alibis.

---

**Remember**: With great power comes great liability insurance premiums. Choose your heroes wisely. 🦸‍♂️

*P.S. - If you're reading this, Bruce, we can talk. Our lawyers are available Monday through Friday, 9-5. Please use the front door.*