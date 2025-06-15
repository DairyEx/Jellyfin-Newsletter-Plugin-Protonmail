# 📬 Jellyfin Newsletter Plugin – ProtonMail Fork

This is a fork of the [Cloud9Developer/Jellyfin-Newsletter-Plugin](https://github.com/Cloud9Developer/Jellyfin-Newsletter-Plugin) modified to support **ProtonMail SMTP** via API token authentication using **MailKit**.

---

## 🎯 Purpose

The plugin automatically scans your Jellyfin media library, gathers newly added items (Movies, TV Shows, Music), and sends them in a beautifully formatted HTML email newsletter.

### ✅ Why This Fork?

- Supports **ProtonMail SMTP** using app-specific tokens
- Uses **MailKit** for secure and reliable email delivery
- Compatible with **Jellyfin 10.10.7**
- Fully customizable newsletter layout and scheduling
- Supports **manual and automated** newsletter sends

---

## 🛠️ Installation

### Option 1: Manual ZIP Install
1. Download the latest release: [Releases](https://github.com/DairyEx/Jellyfin-Newsletter-Plugin-Protonmail/releases)
2. In Jellyfin:  
   **Dashboard → Plugins → Manual → Choose ZIP File**
3. Restart Jellyfin after installing
4. Go to **Dashboard → Plugins → Newsletters** to configure your ProtonMail settings:
   - SMTP Server: `smtp.protonmail.com`
   - Port: `587` (STARTTLS) or `465` (SSL)
   - Username: Your ProtonMail address
   - Password: ProtonMail App Password (Token)

---

## 📦 Repository Manifest

If you're installing this plugin through a custom repository manifest:

```json
https://raw.githubusercontent.com/DairyEx/Jellyfin-Newsletter-Plugin-Protonmail/master/manifest.json
