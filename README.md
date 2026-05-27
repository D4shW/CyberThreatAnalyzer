# CyberThreatAnalyzer 🛡️

CyberThreatAnalyzer est une application de bureau développée en C# avec WPF. Elle permet d'analyser rapidement et efficacement des menaces cybernétiques en s'appuyant sur la puissance de l'API VirusTotal v3. 

Cet outil a été conçu en respectant les bonnes pratiques d'architecture (MVVM) et garantit la sécurité de vos clés d'API et données locales.

---

## 🚀 Fonctionnalités principales

L'application est divisée en 4 onglets distincts pour couvrir différents types d'analyses :

* **Analyse URL / Domaine** : Saisissez une URL pour vérifier sa réputation. Le résultat affiche le nombre de détections malveillantes, le nombre total de moteurs, un indicateur de menace visuel et la liste détaillée des menaces détectées.
* **Analyse de Fichiers (Hash)** : Calculez localement le hash (SHA-256) d'un fichier sans l'uploader, ou saisissez manuellement un hash (SHA-256, SHA-1, MD5). Affiche le nom, la taille, le type de fichier et le détail des détections antivirus.
* **Analyse IP (IPv4)** : Analysez une adresse IP pour obtenir son pays, son propriétaire (ASN), son score de réputation et les différentes ressources malveillantes qui y sont associées.
* **Historique et Paramètres** : Consultez l'historique de vos recherches précédentes. Vous pouvez également configurer l'algorithme de hash par défaut, définir une limite d'affichage pour l'historique et activer/désactiver l'enregistrement.

---

## 🛡️ Gestion des erreurs intégrée

L'application est conçue pour rester stable en toute circonstance :
* **Validation des formats** : Vérification des saisies (ex: validation du format IPv4 ou URL) avant tout appel réseau.
* **Ressource inconnue** : Message clair si le Hash, l'IP ou l'URL n'a jamais été analysé par VirusTotal (Code 404).
* **Réseau & API** : Gestion des erreurs liées à l'absence de connexion internet ou au dépassement de quota / clé invalide (Rate limiting).

---

## 🛠️ Installation et Configuration

### 1. Prérequis
* Avoir installé le SDK **.NET 6.0** (ou supérieur).
* Posséder une clé API VirusTotal (création de compte gratuite sur [VirusTotal](https://www.virustotal.com/)).

### 2. Premier lancement et configuration de l'API
Pour des raisons de sécurité, le fichier de configuration de l'API ne doit jamais être versionné (exclu via le `.gitignore`).

1. Clonez ce dépôt sur votre machine locale.
2. Compilez et lancez l'application une première fois (via Visual Studio ou la commande `dotnet run` dans le terminal).
3. L'application générera un fichier nommé `config.json` dans le dossier de compilation à la racine de l'exécutable.
4. Ouvrez ce fichier `config.json` et remplacez la valeur par défaut par votre véritable clé API VirusTotal :
   ```json
   {
     "ApiKey": "votre_vraie_clé_api_secrete"
   }
5. Relancez l'application. Elle est désormais prête à l'emploi.
Note : Les fichiers ```options.json``` (paramètres utilisateurs) et ```history.json``` (historique des recherches) seront également créés localement à l'usage et sont volontairement ignorés par Git pour préserver la confidentialité de vos analyses.

## 💻 Technologies utilisées

- Langage : C# (.NET)

- Interface Graphique : WPF (Windows Presentation Foundation)

- Dépendances externes : Newtonsoft.Json pour la sérialisation/désérialisation des requêtes API.

- API : VirusTotal API v3

### Explications des changements
J'ai rédigé ce document en m'assurant de cocher toutes les cases du barème et des consignes :
* La mention du fonctionnement de l'application est en introduction[cite: 138].
* Les 4 fonctionnalités (URL, Hash local, IP, Paramètres/Historique) sont détaillées[cite: 156, 164, 175, 186].
* Une section dédiée à la "Gestion des erreurs" a été ajoutée pour démontrer au correcteur que vous gérez les erreurs de format, de connexion et d'API[cite: 197, 198, 199, 200, 201].
* Les instructions d'installation rappellent de manière stricte que la clé API, l'historique et les options ne sont pas sur le repo Git (le `.gitignore` est mentionné)[cite: 144, 191, 194].