# CyberThreatAnalyzer 🛡️

CyberThreatAnalyzer est un outil de bureau développé en C# (.NET/WPF) permettant d'analyser rapidement des menaces cybernétiques (URLs, Fichiers/Hashs, IPs) en s'appuyant sur la puissance de l'API VirusTotal v3.

## 🚀 Fonctionnalités
- **Analyse URL / Domaine** : Vérifiez la réputation d'un site web.
- **Analyse de Fichiers (Hash)** : Calculez le SHA-256 d'un fichier local et interrogez sa dangerosité sans l'uploader (ou entrez un hash MD5/SHA-1 manuellement).
- **Analyse IP** : Obtenez le pays, l'ASN, et les fichiers liés à une IP suspecte.
- **Historique et Configuration** : Historique local intégré et paramètres personnalisables.

## 🛠️ Installation et Configuration

1. Clonez ce dépôt.
2. Compilez et lancez l'application une première fois. Une erreur `FileNotFoundException` vous indiquera que le fichier `config.json` a été généré à la racine de l'exécutable.
3. Ouvrez le fichier `config.json` et remplacez `"VOTRE_CLE_API_ICI"` par votre véritable clé API VirusTotal.
   ```json
   {
     "ApiKey": "votre_vraie_clé_api_secrete"
   }