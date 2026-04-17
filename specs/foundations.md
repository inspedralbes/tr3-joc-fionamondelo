# Foundations — Implementació del commutador de tema per al login

## 1. Context del projecte

Videojoc 2D bomberman multijugador desenvolupat amb **Unity** (client) i **Node.js** (backend), seguint una **arquitectura de microserveis** amb comunicació híbrida mitjançant HTTP i WebSockets.

El backend fa servir el **patró Repository** per a l'accés a dades i s'integra amb **ML-Agents** d'Unity per a la intel·ligència artificial dels bots.

El joc base està **totalment desenvolupat** i en funcionament. Aquest document defineix els fonaments per a una nova funcionalitat que s'afegirà sense afectar l'estabilitat del sistema existent.

---

## 2. Objectiu de la funcionalitat

Implementar un **commutador de tema** (toggle) a la pantalla de login del jugador que permeti alternar entre:

| Tema | Descripció |
|------|------------|
| **Mode dia** | Paleta clara, fons lluminós, contrast alt |
| **Mode nit** | Paleta fosca, fons fosc, menor fatiga visual |

El canvi de tema ha de ser **instantani**, **persistent** (recordar la preferència de l'usuari) i **transversal** a tota la pantalla de login.

---

## 3. Restriccions tècniques i de disseny

### 3.1 Idioma

- Tota la **documentació**, **interfície d'usuari** i **missatges del sistema** han de ser **estrictament en català**.
- Etiquetes, botons, descripcions i errors s'han de mostrar en català.

### 3.2 Seguretat

- El sistema de canvi de tema **no ha d'interferir** amb els mecanismes d'autenticació ni amb la seguretat del login.
- La selecció del tema **no emmagatzema dades sensibles**; és una preferència estrictament visual.
- No es poden exposar contrasenyes, tokens ni dades personals a través del tema.
- El tema escollit s'ha d'emmagatzemar **localment** (preferències del navegador/client) o bé a la base de dades de preferències sense vinculació directa amb credencials.

### 3.3 Rendiment

- El canvi de tema s'ha de renderitzar **sense parpadeig** (flash-free).
- La transició ha de ser **smooth** (≤ 300ms).

### 3.4 Compatibilitat

- El tema s'ha de respectar en **tots els dispositius** on s'executi el client Unity (escriptori, mòbil, tablet).
- La implementació ha de ser **responsive** i accessible (contrast mínim WCAG AA).

### 3.5 Integració amb el sistema existent

- La funcionalitat ha de ser **modular** i **no intrusiva** respecte al codi del login existent.
- No es modifica cap endpoint d'API ni la lògica d'autenticació.
- El canvi de tema s'aplica **exclusivament** a la pantalla de login; la resta del joc manté el tema per defecte.

---

## 4. Criteris d'acceptació

| Criteri | Descripció |
|---------|------------|
| CA-1 | El botó de commutació de tema és visible a la pantalla de login. |
| CA-2 | Alternar el tema canvia la paleta de colors de tots els elements del login sense recarregar la pàgina. |
| CA-3 | La selecció del tema es manté en sessions posteriors de l'usuari. |
| CA-4 | Canviar el tema no afecta el procés d'autenticació ni la validesa de les credencials. |
| CA-5 | Tots els textos de la interfície de login i d'aquesta funcionalitat són en català. |
| CA-6 | El contrast dels colors en ambdós temes compleix WCAG AA (relació ≥ 4.5:1). |
| CA-7 | La funcionalitat no genera errors ni warnings a la consola. |

---

## 5. Decisions de disseny obertes

> En aquesta secció es deixen obertes decisions que es resoldran durant l'especificació de detall:

- **FOD-1**: S'emmagatzemarà la preferència de tema al `localStorage` del client o s'enviarà al backend per persistència?
- **FOD-2**: Es permetrà al jugador personalitzar colors individuals o només alternar entre tema clar i fosc predefinits?
- **FOD-3**: S'afegirà una animació d'icona lunar/solar juntament amb el botó de commutació?

---

*Document creat seguint la metodologia Spec-Driven Development.*
