//
//  MainWindow.UI.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Reflection;
using Gdk;
using Gtk;
using Launchpad.Launcher.Utility;
using Pango;
using UIElement = Gtk.Builder.ObjectAttribute;

// ReSharper disable UnassignedReadonlyField
#pragma warning disable 169
#pragma warning disable 649
#pragma warning disable 1591
#pragma warning disable SA1134 // Each attribute should be placed on its own line of code
#pragma warning disable SA1214 // Readonly fields should appear before non-readonly fields

namespace Launchpad.Launcher.Interface
{
	/// <summary>
	/// Interface elements for the <see cref="MainWindow"/> widget.
	/// </summary>
	public partial class MainWindow
	{
		[UIElement] private readonly ImageMenuItem MenuRepairItem;
		[UIElement] private readonly ImageMenuItem MenuReinstallItem;
		[UIElement] private readonly ImageMenuItem MenuAboutItem;

		[UIElement] private readonly TextView ChangelogTextView;
		[UIElement] private readonly Image BannerImage;

		[UIElement] private readonly Label StatusLabel;

		[UIElement] private readonly ProgressBar MainProgressBar;
		[UIElement] private readonly Button MainButton;

		[UIElement] private Dialog LoginDialog;
		[UIElement] private Entry LoginUser;
		[UIElement] private Entry LoginPassword;
		[UIElement] private Image LoginIcon;

		/*
		[UIElement] private readonly Button LoginButton;
		[UIElement] private readonly Button LoginExitButton;
		[UIElement] private readonly Button LoginUser;
		[UIElement] private readonly Button LoginPassword;
		*/

		/// <summary>
		/// Creates a new instance of the <see cref="MainWindow"/> class, loading its interface definition from file.
		/// </summary>
		/// <returns>An instance of the main window widget.</returns>
		public static MainWindow Create()
		{
			using (var builder = new Builder(Assembly.GetExecutingAssembly(), "Launchpad.Launcher.Interface.Launchpad.glade", null))
			{
				var window = new MainWindow(builder, builder.GetObject(nameof(MainWindow)).Handle)
				{
					Icon = ResourceManager.ApplicationIcon
				};

				return window;
			}
		}

		/// <summary>
		/// Binds UI-related events.
		/// </summary>
		private void BindUIEvents()
		{
			this.DeleteEvent += OnDeleteEvent;
			this.MenuReinstallItem.Activated += OnReinstallGameActionActivated;
			this.MenuRepairItem.Activated += OnMenuRepairItemActivated;
			this.MenuAboutItem.Activated += OnMenuAboutItemActivated;

			this.MainButton.Clicked += OnMainButtonClicked;
		}

		/// <summary>
		/// Displays the about window to the user.
		/// </summary>
		/// <param name="sender">The sending object.</param>
		/// <param name="e">The event args.</param>
		private void OnMenuAboutItemActivated(object sender, EventArgs e)
		{
			using (var builder = new Builder(Assembly.GetExecutingAssembly(), "Launchpad.Launcher.Interface.Launchpad.glade", null))
			{
				using (var dialog = new AboutDialog(builder.GetObject("MainAboutDialog").Handle))
				{
					dialog.Icon = ResourceManager.ApplicationIcon;
					dialog.Logo = ResourceManager.ApplicationIcon;
					dialog.Run();
				}
			}
		}

		private void CreateLogin()
		{
			using (var builder = new Builder(Assembly.GetExecutingAssembly(), "Launchpad.Launcher.Interface.Launchpad.glade", null))
			{
				using (this.LoginDialog = new Gtk.AboutDialog(builder.GetObject("LoginDialog").Handle))
				{
					var loginButton = (Gtk.Button)builder.GetObject("Login");
					loginButton.Clicked += OnLoginButtonClickedLogin;
					this.LoginDialog.Destroyed += OnExitButtonClickedLogin;
					this.LoginUser = (Gtk.Entry)builder.GetObject("LoginUser1");
					this.LoginPassword = (Gtk.Entry)builder.GetObject("LoginPassword1");
					this.LoginIcon = (Gtk.Image)builder.GetObject("LoginIcon");
					this.LoginIcon.Icon = ResourceManager.ApplicationIcon;
					this.LoginDialog.Run();
				}
			}
		}

		private void OnExitButtonClickedLogin(object sender, EventArgs e)
		{
			Application.Quit();
		}

		private void OnLoginButtonClickedLogin(object sender, EventArgs e)
		{
			// MANEJAR HTTP POST A LOGIN ACA
			if (this.LoginUser.Text.Equals("admin") && this.LoginPassword.Text.Equals("admin"))
			{
				this.LoginDialog.Destroyed -= OnExitButtonClickedLogin;
				this.LoginDialog.Hide();
			}
			else
			{
				ShowLoginMessage("Usuario y/o contraseña incorrectos.\n\n" + "Intente nuevamente.");
			}
		}

		private void ShowLoginMessage(string message)
		{
			using (var badLoginDialog = new MessageDialog(
			this,
			DialogFlags.Modal,
			MessageType.Warning,
			ButtonsType.Ok,
			LocalizationCatalog.GetString
			(
				message
			)
			))
			{
				badLoginDialog.Run();
			}
		}

		/// <summary>
		/// Exits the application properly when the window is deleted.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="a">The alpha component.</param>
		private static void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}
	}
}
