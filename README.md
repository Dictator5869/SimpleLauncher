<h1>SimpleLauncher</h1>
<p><strong>Version 2 Now Available</strong></p>

<p><strong>SimpleLauncher</strong> is a lightweight, customizable Windows launcher designed for children. It provides a controlled environment where only approved applications are accessible—without needing a Microsoft account or registry tweaks.</p>

<h2>✨ Key Features</h2>
<ul>
  <li>Child-friendly full-screen launcher</li>
  <li>No Microsoft account required (local accounts supported)</li>
  <li>No registry edits needed in Version 2</li>
  <li>Optional parental code for protected apps (default: <code>9678</code>)</li>
  <li>Customizable layout and background</li>
  <li>Optional auto-start at login</li>
</ul>

<h2>🛠️ Setup Instructions</h2>

<h3>🔹 Running at Startup</h3>
<p>To auto-launch SimpleLauncher on login:</p>
<ol>
  <li>Create a shortcut to <code>SimpleLauncher.exe</code>.</li>
  <li>Place it in:<br>
    <code>%AppData%\Microsoft\Windows\Start Menu\Programs\Startup</code>
  </li>
</ol>

<h3>🔹 Adding and Configuring Shortcuts</h3>
<p>Edit <code>shortcuts.ini</code> in the application folder. Example:</p>

<pre><code>[Shortcut1]
FilePath="C:\Users\username\Desktop\Roblox.lnk"
X=100
Y=200
RequiresCode=false
</code></pre>

<ul>
  <li><strong>FilePath</strong>: Path to the <code>.exe</code> or shortcut</li>
  <li><strong>X, Y</strong>: Screen coordinates for placement</li>
  <li><strong>RequiresCode</strong>: Set to <code>true</code> to require the parental code</li>
</ul>

<h3>🔹 Changing the Background</h3>
<p>Replace the existing <code>Background.jpg</code> in the app directory with your own image. File name must remain <code>Background.jpg</code>.</p>

<h2>🔐 Parental Control</h2>
<p>You can lock specific shortcuts with a code prompt.</p>

<div style="background-color: #fffbe6; padding: 10px; border-left: 4px solid #ffc107;">
  <strong>Default parental code:</strong> <code>9678</code>
</div>

<h2>📁 Legacy Info: Version 1 (End of Life)</h2>
<p>Version 1 required modifying the system shell to launch SimpleLauncher instead of Explorer. This is no longer needed in Version 2.</p>

<h3>🧾 Shell Replacement (V1 Only)</h3>
<p>To fully replace the desktop shell (not recommended now):</p>
<ol>
  <li>Open <code>regedit</code></li>
  <li>Navigate to:<br>
    <code>HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon</code>
  </li>
  <li>Change the <code>Shell</code> value to:<br>
    <code>C:\YourLocation\SimpleLauncher\SimpleLauncher.exe</code>
  </li>
</ol>