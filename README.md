<div align="center">
  <img src="docs/img/logo.png" alt="BombermanTR3 Logo" width="200" />
  <h1>💣 BombermanTR3 💣</h1>
  <p><strong>Joc Multijugador en Temps Real amb Arquitectura de Microserveis</strong></p>

  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![Unity](https://img.shields.io/badge/Unity-2D-blue?logo=unity)](https://unity.com/)
  [![Node.js](https://img.shields.io/badge/Node.js-v18+-green?logo=node.js)](https://nodejs.org/)
  [![Docker](https://img.shields.io/badge/Docker-Enabled-blue?logo=docker)](https://www.docker.com/)
</div>

---

## 👤 Integrant
* **Fiona Mondelo Giaramita**

---

## 📝 Descripció
**BombermanTR3** és un projecte de joc multijugador clàssic evolucionat per a la web moderna. Utilitza una arquitectura de microserveis per gestionar usuaris, partides i comunicació en temps real. El client està desenvolupat en **Unity 2D**, connectant-se a un backend robust en **Node.js** mitjançant **WebSockets** per a una experiència de joc fluida i sense retards.

---

## 🚀 Accés al Projecte
* **🌍 URL de Producció:** [http://204.168.211.255:8080](http://204.168.211.255:8080)
* **📊 Gestió de Tasques:** [Taiga Project](https://tree.taiga.io/project/66fiona66-dam_fionamondelo/timeline)

---

## 🖼️ Diagrames del Projecte
En aquesta secció pots trobar tota la documentació gràfica de l'arquitectura i el disseny del sistema:

| Diagrama | Enllaç |
| :--- | :--- |
| **Casos d'Ús** | [Veure Diagrama](./Diagrames/DiagramaCasosUs.png) |
| **Entitat-Relació** | [Veure Diagrama](./Diagrames/DiagramaEntitatRelació.png) |
| **Microserveis** | [Veure Diagrama](./Diagrames/DiagramaMicroserveis.png) |

> [!TIP]
> Pots trobar tots els fitxers originals a la carpeta [/Diagrames](./Diagrames).

---

## 🛠️ Tecnologies Utilitzades

### **Client (Frontend)**
* **Unity 2D & C#**: Motor de joc i lògica de client.
* **UnityWebRequest**: Comunicació amb l'API REST.
* **ClientWebSocket**: Sincronització de moviments i accions en temps real.

### **Backend (Microserveis)**
* **Node.js & Express.js**: Framework per als serveis d'usuaris i partides.
* **MongoDB & Mongoose**: Base de dades NoSQL per a la persistència de dades.
* **WebSocket (ws)**: Servidors de comunicació bidireccional.
* **Bcrypt**: Seguretat i xifrat de contrasenyes.

### **Infraestructura**
* **Docker & Docker Compose**: Contenidorització de tots els serveis.
* **Nginx**: Actua com a API Gateway i reverse proxy.

---

## 📂 Estructura de Carpetes
```text
tr3-joc-fionamondelo/
├── 📁 BombermanTR3/           # Projecte Unity (Client)
├── 📁 Diagrames/              # Documentació gràfica (PNG)
├── 📁 docs/                   # Logs de prompts i imatges
├── 📁 serveis/                # Microserveis Backend
│   ├── 📁 usuaris/            # Gestió de perfils i Auth
│   ├── 📁 partides/           # Lògica de sales i resultats
│   └── 📁 websocket/          # Sincronització en temps real
├── 📁 nginx/                  # Configuració de l'API Gateway
├── docker-compose.yml         # Orquestració de contenidors
└── .env                       # Variables d'entorn
```

---

## 🔧 Instal·lació i Execució

### **Backend**
1. Clona el repositori.
2. Configura el fitxer `.env` amb la teva `MONGO_URI`.
3. Executa la infraestructura amb Docker:
   ```bash
   docker-compose up --build
   ```

### **Client**
1. Obre la carpeta `BombermanTR3` amb Unity Hub (versió recomanada 2022.3+).
2. Assegura't que la IP al `ApiManager.cs` apunta a `localhost` o a la IP del servidor.
3. Prem **Play**!

---

<div align="center">
  <sub>Desenvolupat per Fiona Mondelo - 2026</sub>
</div>
