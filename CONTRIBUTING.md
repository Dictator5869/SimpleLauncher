# Contributing to SimpleLauncher

Thanks for your interest in contributing to **SimpleLauncher**! 🎉  
This project is designed to be simple, accessible, and helpful — and your support makes it even better.

---

## 📝 How to Contribute

### 1. 🐛 Report Bugs

If you’ve found a bug or unexpected behavior:

- Check the [issues page](https://github.com/Dictator5869/SimpleLauncher/issues) to see if it’s already been reported.
- If not, open a new issue and include:
  - A clear description of the bug.
  - Steps to reproduce it.
  - Screenshots or error messages, if possible.
  - Your environment (Windows version, .NET version, etc.).

---

### 2. 💡 Suggest Features

Got an idea to improve SimpleLauncher?

- Create a [feature request](https://github.com/Dictator5869/SimpleLauncher/issues/new?labels=enhancement) and explain:
  - What it does.
  - Why it would be useful.
  - How it fits with the current launcher design and user experience.

Here are some ideas we're interested in:
- Locking keyboard shortcuts like Alt+Tab or Ctrl+Esc.
- Multi-user profiles for different children.
- Custom sounds or theme settings.
- Support for translations/localization.
- Reset launcher to factory defaults from within the UI.

---

### 3. 🧪 Contribute Code

We welcome pull requests! Here's how to get started:

#### 🛠️ Set Up Your Environment
You'll need:
- Visual Studio 2022 or newer
- .NET Framework (e.g., 4.8)
- A Windows system for testing (Windows 10 or 11 preferred)

> **Note:** This repository does **not** include Visual Studio solution (`.sln`) or project (`.csproj`) files.  
> You will need to create these yourself before building the project.  
>  
> To do this:
> 1. Open Visual Studio and create a new Windows Forms App (.NET Framework) project.
> 2. Add the existing source files from this repository into your new project.
> 3. Configure any necessary references (e.g., System.Windows.Forms).
> 4. Build and run your project.

#### 🧱 Fork & Branch
1. Fork this repository.
2. Clone your fork:
   ```bash
   git clone https://github.com/yourusername/SimpleLauncher.git
   cd SimpleLauncher
   git checkout -b your-feature-name
   ```

#### 🔨 Make Your Changes
- Follow the existing coding style.
- Use clear naming for variables and methods.
- Comment where necessary, especially for non-obvious logic.
- Test your changes — especially for edge cases like multiple monitors, DPI scaling, and password behavior.

#### ✅ Commit & Push
```bash
git commit -m "Add: Short description of what you changed"
git push origin your-feature-name
```

#### 📬 Open a Pull Request
- Describe what you changed and why.
- Link to the related issue if applicable.
- Be open to review feedback and iterate if needed.

---

## 🧼 Code Style & Guidelines

To keep everything clean and consistent:

- Use **PascalCase** for public methods and properties.
- Use **camelCase** for local variables and private fields.
- Keep methods short and focused.
- Avoid hardcoding paths or values; use configs or constants where appropriate.
- Don’t commit compiled files (`bin/`, `obj/`) or `.user`/`.suo` files.

---

## 🛡️ Code of Conduct

This project follows the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md).  
Please be kind, respectful, and constructive when interacting with others.  
We’re all here to build something better together.

---

## 🙌 Thank You

Whether you're reporting bugs, suggesting improvements, writing code, or just spreading the word — **thank you**!  
Your input helps make SimpleLauncher a better experience for families and kids everywhere.

Happy coding! 💻✨
