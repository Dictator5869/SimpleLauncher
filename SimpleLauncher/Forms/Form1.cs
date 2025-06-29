using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

public class Form1 : Form
{
    private static readonly string IniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shortcuts.ini");
    private static readonly string BackgroundImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Background.jpg");
    private static readonly string FolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Parent Apps");

    // External function for window manipulation
    [DllImport("user32.dll")]
    private static extern int FindWindow(string className, string windowTitle);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(int hwnd, int command);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;
    private const uint SPI_SETSCREENSAVEACTIVE = 0x0011;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_SHOWWINDOW = 0x0040;

    private Button exitButton;
    private Button parentButton;
    private IntPtr hwndDesktop;

    public Form1()
    {
        // Form initialization
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;

        // Set background image
        SetBackgroundImage();

        // Load shortcuts
        LoadShortcutsFromIni();

        // Add Exit Button
        CreateExitButton();

        // Add Parent Button
        CreateParentButton();

        // Disable Start Menu settings on start
        HideTaskbar();
        WinKeyInterceptor.Enable();


        // Store the desktop window handle to ensure it's always on top
        hwndDesktop = (IntPtr)FindWindow("Progman", null);
    }

    private void SetBackgroundImage()
    {
        if (File.Exists(BackgroundImagePath))
        {
            this.BackgroundImage = Image.FromFile(BackgroundImagePath);
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        else
        {
            this.BackColor = Color.Black;
        }
    }

    private void LoadShortcutsFromIni()
    {
        if (!File.Exists(IniFilePath))
        {
            MessageBox.Show($"The INI file '{IniFilePath}' does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        IniFile ini = new IniFile(IniFilePath);
        int index = 1;

        while (true)
        {
            string section = $"Shortcut{index}";
            string filePath = ini.Read(section, "FilePath");
            if (string.IsNullOrEmpty(filePath)) break;

            int x = int.Parse(ini.Read(section, "X", "0"));
            int y = int.Parse(ini.Read(section, "Y", "0"));
            bool requiresCode = ini.Read(section, "RequiresCode", "false").ToLower() == "true";

            CreateShortcut(filePath, new Point(x, y), requiresCode);
            index++;
        }
    }

    private void CreateShortcut(string filePath, Point location, bool requiresCode)
    {
        Panel shortcutPanel = new Panel
        {
            Location = location,
            Size = new Size(100, 120),
            BackColor = Color.Transparent
        };

        PictureBox iconBox = new PictureBox
        {
            Image = GetFileIcon(filePath),
            Size = new Size(100, 100),
            SizeMode = PictureBoxSizeMode.Zoom,
            Dock = DockStyle.Top,
            Tag = new { FilePath = filePath, RequiresCode = requiresCode }
        };

        iconBox.Click += (s, e) =>
        {
            dynamic tag = ((PictureBox)s).Tag;
            string targetPath = tag.FilePath;
            bool requiresCode = tag.RequiresCode;

            if (requiresCode)
            {
                string inputCode = PromptForCode();
                if (inputCode != "9678") // Replace with your code logic
                {
                    MessageBox.Show("Incorrect code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (File.Exists(targetPath) || Directory.Exists(targetPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetPath,
                    UseShellExecute = true
                });
            }
        };

        Label shortcutLabel = new Label
        {
            Text = Path.GetFileNameWithoutExtension(filePath),
            Dock = DockStyle.Bottom,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoEllipsis = true,
            BackColor = Color.Transparent,
            ForeColor = Color.White,
            Height = 20
        };

        shortcutPanel.Controls.Add(iconBox);
        shortcutPanel.Controls.Add(shortcutLabel);
        this.Controls.Add(shortcutPanel);
    }

    private string PromptForCode()
    {
        using (Form prompt = new Form())
        {
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Enter Code";

            Label textLabel = new Label { Left = 20, Top = 20, Text = "Enter the code:" };
            TextBox inputBox = new TextBox { Left = 20, Top = 50, Width = 240, PasswordChar = '*' };
            Button confirmationButton = new Button { Text = "OK", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmationButton);
            prompt.AcceptButton = confirmationButton;

            prompt.TopMost = true; // Make sure this prompt always appears on top

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
        }
    }

    private Image GetFileIcon(string filePath)
    {
        try
        {
            Icon icon = Icon.ExtractAssociatedIcon(filePath);
            return icon?.ToBitmap() ?? SystemIcons.Application.ToBitmap();
        }
        catch
        {
            return SystemIcons.Application.ToBitmap();
        }
    }

    public void HideTaskbar()
    {
        int taskbarHandle = FindWindow("Shell_TrayWnd", null);
        if (taskbarHandle != 0)
        {
            ShowWindow(taskbarHandle, SW_HIDE);
        }
    }

    public void ShowTaskbar()
    {
        int taskbarHandle = FindWindow("Shell_TrayWnd", null);
        if (taskbarHandle != 0)
        {
            ShowWindow(taskbarHandle, SW_SHOW);
        }
    }

    private void CreateExitButton()
    {
        // Create the exit button and add it to the form
        exitButton = new Button
        {
            Text = "Exit",
            Location = new Point(this.ClientSize.Width - 80, 10),
            Size = new Size(70, 30),
            BackColor = Color.Red,
            ForeColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        exitButton.Click += (s, e) =>
        {
            // Prompt for parental code before allowing exit
            string inputCode = PromptForCode();
            if (inputCode == "9678") // Replace with your parental code logic
            {
                // If the code is correct, close the application
                int hwnd = FindWindow("Shell_TrayWnd", null);
                if (hwnd != 0)
                {
                    ShowWindow(hwnd, SW_SHOW); // Show the taskbar
                }
                this.Close(); // Close the application
            }
            else
            {
                MessageBox.Show("Incorrect code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        this.Controls.Add(exitButton); // Add the exit button to the form
    }

    private void CreateParentButton()
    {
        parentButton = new Button
        {
            Text = "Parent",
            Size = new Size(70, 30),
            BackColor = Color.Blue,
            ForeColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Left // Ensure it stays anchored to the top-left
        };

        // Place the button initially in the top-left corner
        parentButton.Location = new Point(10, 10);

        parentButton.Click += (s, e) =>
        {
            // Lock behind the parental code
            string inputCode = PromptForCode();
            if (inputCode != "9678") // Replace with your parental code logic
            {
                MessageBox.Show("Incorrect code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Use FolderPath for the folder path
            if (Directory.Exists(FolderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = FolderPath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        this.Controls.Add(parentButton);
    }

    // Block all window keyboard shortcuts (like Alt+F4, Win key, etc.)
    protected override void WndProc(ref Message m)
    {
        const int WM_SYSCOMMAND = 0x112;
        const int SC_MINIMIZE = 0xF020;
        const int SC_CLOSE = 0xF060;
        const int WM_KEYDOWN = 0x100;
        const int VK_F4 = 0x73;

        if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt32() == SC_MINIMIZE || m.WParam.ToInt32() == SC_CLOSE))
        {
            return; // Prevent minimize and close
        }

        if (m.Msg == WM_KEYDOWN)
        {
            int keyCode = m.WParam.ToInt32();
            if (keyCode == VK_F4)
            {
                return; // Block Alt+F4
            }
        }

        base.WndProc(ref m);
    }

    // Ensure the application always stays above the desktop window
    private void UpdateAlwaysOnTop()
    {
        SetWindowPos(this.Handle, hwndDesktop, 0, 0, 0, 0, SWP_NOZORDER | SWP_SHOWWINDOW);
    }

    public class WinKeyInterceptor
    {
        private static IntPtr hookId = IntPtr.Zero;
        private static LowLevelKeyboardProc proc = HookCallback;

        public static void Enable()
        {
            hookId = SetHook(proc);
        }

        public static void Disable()
        {
            UnhookWindowsHookEx(hookId);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Block LWIN and RWIN keys
                if (vkCode == (int)Keys.LWin || vkCode == (int)Keys.RWin)
                {
                    return (IntPtr)1; // Block the key
                }
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private const int WH_KEYBOARD_LL = 13;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk,
            int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Optionally, restore taskbar visibility
        int hwnd = FindWindow("Shell_TrayWnd", null);
        if (hwnd != 0)
        {
            ShowWindow(hwnd, SW_SHOW); // Show the taskbar
        }

        // restore Start menu settings
        ShowTaskbar();
        WinKeyInterceptor.Disable();

        WinKeyInterceptor.Disable(); // Remove hook
        Application.ExitThread();    // Clean exit
        Process.GetCurrentProcess().Kill(); // Failsafe kill

        base.OnFormClosing(e);
    }
}
