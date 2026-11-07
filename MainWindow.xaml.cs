using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WarLikeOSW;

public partial class MainWindow : Window
{
    private sealed record Entry(string Name, string Description, string Target);
    private readonly List<Entry> services = new()
    {
        new("WarLike Hub", "Portail de l’écosystème", "https://warlike-hub.pages.dev/"),
        new("WarLike Engine", "Création de jeux", "https://warlike-engine.pages.dev/"),
        new("WarLike Studio", "Production musicale", "https://warlike-studio.pages.dev/"),
        new("WarLike Library", "Ressources et assets", "https://warlike-library.pages.dev/"),
        new("WarLike Asset Stores", "Assets et extensions", "https://warlike-asset-stores.pages.dev/"),
        new("WarLike Modeliser", "Modélisation", "https://warlike-modeliser.pages.dev/"),
        new("WarLike Node", "Compositing visuel", "https://warlike-node.pages.dev/"),
        new("WarLike Monteur", "Montage vidéo", "https://warlike-monteur.pages.dev/"),
        new("WarLike MISC", "Apprentissage musical", "https://warlike-misc.pages.dev/"),
        new("WarLike Radio", "Radios et émissions", "https://warlike-radio.pages.dev/"),
        new("WarLike Chat", "Salons et messages", "https://warlike-chat.pages.dev/"),
        new("WarLike Mail", "Messagerie", "https://warlike-mail.vikktor7.chatgpt.site/"),
        new("WarLike Server", "Infrastructure", "https://warlike-server.vikktor7.chatgpt.site/serveur"),
        new("WarLike PDF", "Outils PDF", "https://warlike-pdf.pages.dev/"),
        new("WarLike Crypte", "Coffre personnel", "https://warlike-crypte.pages.dev/"),
        new("WarLike Gaming", "Jeux et prototypes", "https://game-hub-v2.pages.dev/"),
        new("WarLike Android Hub", "Application Android", "https://warlike-android-hub.pages.dev/")
    };
    private readonly List<Entry> installed = new();
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMinutes(1) };

    public MainWindow()
    {
        InitializeComponent();
        timer.Tick += (_, _) => UpdateClock();
        timer.Start();
        UpdateClock();
        ScanApps();
        ApplyFilter();
    }
    private void UpdateClock() => Clock.Text = DateTime.Now.ToString("dddd d MMMM  ·  HH:mm");
    private void ScanApps()
    {
        installed.Clear();
        var roots = new[] { Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                            Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) };
        foreach (var root in roots.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!Directory.Exists(root)) continue;
            try
            {
                foreach (var path in Directory.EnumerateFiles(root, "*.lnk", SearchOption.AllDirectories))
                    installed.Add(new Entry(Path.GetFileNameWithoutExtension(path), "Raccourci Windows", path));
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
        installed.Sort((a, b) => StringComparer.CurrentCultureIgnoreCase.Compare(a.Name, b.Name));
    }
    private void ApplyFilter()
    {
        if (Search is null || Services is null || Installed is null) return;
        string term = Search.Text.Trim();
        Services.ItemsSource = services.Where(x => x.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase) || x.Description.Contains(term, StringComparison.CurrentCultureIgnoreCase)).ToList();
        Installed.ItemsSource = installed.Where(x => x.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase)).ToList();
    }
    private void SearchChanged(object sender, TextChangedEventArgs e) => ApplyFilter();
    private void RefreshApps(object sender, RoutedEventArgs e) { ScanApps(); ApplyFilter(); }
    private void OpenService(object sender, MouseButtonEventArgs e) { if (Services.SelectedItem is Entry item) Open(item.Target); }
    private void OpenInstalled(object sender, MouseButtonEventArgs e) { if (Installed.SelectedItem is Entry item) Open(item.Target); }
    private void Open(string target)
    {
        try { Process.Start(new ProcessStartInfo(target) { UseShellExecute = true }); }
        catch (Exception error) { MessageBox.Show(this, "Impossible d’ouvrir cet élément : " + error.Message, "WarLike OSW", MessageBoxButton.OK, MessageBoxImage.Warning); }
    }
}
