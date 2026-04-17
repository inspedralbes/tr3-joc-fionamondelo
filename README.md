# BombermanTR3 - Joc Multijugador en Temps Real

## Integrants
* Fiona Mondelo Giaramita 

---

## Nom del Projecte
**BombermanTR3** - Joc multijugador clàssic.

---

## Descripció
Projecte de joc Bomberman multijugador desenvolupat amb **Unity (client)** i **Node.js (backend)** que permet a dos jugadors competir en temps real a través d'una arquitectura de microserveis. El sistema inclou gestió d'usuaris, creació de partides, i sincronització en temps real mitjançant **WebSocket**.

---

## Gestió i Disseny
* **Gestor de Tasques:** 
* **Prototip Gràfic:** 
* **URL de Producció:** Pendent de desplegar

---

## 📂 Esquema de les Carpetes del Projecte
```text
tr3-joc-fionamondelo/  
├── README.md                          
├── docker-compose.yml                   
├── .env                              
├── nginx/                             
│   └── nginx.conf                   
├── serveis/                          
│   ├── usuaris/                       
│   │   ├── package.json  
│   │   ├── server.js  
│   │   └── src/  
│   │       ├── models/  
│   │       │   └── Usuari.js  
│   │       ├── routes/  
│   │       │   └── usuariRoutes.js  
│   │       ├── services/  
│   │       │   └── UsuariService.js  
│   │       └── repositories/  
│   │           └── MongoUsuariRepository.js  
│   ├── partides/                      # Servei de partides (Port 3002)  
│   │   ├── package.json  
│   │   ├── server.js  
│   │   └── src/  
│   │       ├── models/  
│   │       │   ├── Partida.js  
│   │       │   └── Resultat.js  
│   │       ├── routes/  
│   │       │   └── partidaRoutes.js  
│   │       ├── services/  
│   │       │   └── PartidaService.js  
│   │       └── repositories/  
│   │           └── MongoPartidaRepository.js  
│   └── websocket/                     # Servei WebSocket (Port 3003)  
│       ├── package.json  
│       └── server.js  
└── BombermanTR3/                      # Client Unity  
    ├── ProjectSettings/  
    │   └── ProjectSettings.asset  
    ├── Assets/  
    │   └── Scripts/  
    │       ├── Network/  
    │       │   ├── ApiManager.cs  
    │       │   └── WebSocketManager.cs  
    │       ├── UI/  
    │       │   ├── LobbyController.cs  
    │       │   └── ResultsController.cs  
    │       ├── GameManager.cs  
    │       ├── MovementController.cs  
    │       ├── BombController.cs  
    │       ├── PlayerHealth.cs  
    │       └── Destructible.cs  
    └── Packages/

---

## Tecnologies Utilitzades

### **Client**
* Unity 2D & C#
* UnityWebRequest & ClientWebSocket

### **Backend**
* Node.js & Express.js
* MongoDB & Mongoose
* WebSocket (ws)
* Bcrypt & Dotenv

### **Infraestructura**
* Docker & Docker Compose
* Nginx (API Gateway)

---

## Arquitectura
* Microserveis
* API REST
* WebSockets
