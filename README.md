# WarLike OSW

WarLike OSW est un espace de travail natif pour Windows. Il réunit les liens vers les services WarLike et les raccourcis d'applications présents dans les menus Démarrer de l'utilisateur et du PC. Recherche, actualisation et ouverture sont incluses.

C'est une application WPF qui fonctionne sur Windows, pas un système d'exploitation autonome et elle ne remplace pas Explorer ou le système existant. Les services web s'ouvrent dans le navigateur.

## Construire

Sur Windows avec .NET 8 SDK :

`dotnet publish WarLikeOSW.csproj -c Release -r win-x64 --self-contained false -o publish`

Exécuter `publish/WarLikeOSW.exe`. La version ci-dessus nécessite le runtime .NET Desktop 8 x64. GitHub Actions produit un dossier téléchargeable pour les essais. L'installateur, la signature et les tests sur un PC réel restent à réaliser.
