# Spec — Comportament esperat del commutador de tema

Aquest document detalla el comportament funcional i tècnic de la funcionalitat de canvi de tema (dia/nit) a la pantalla de login.

## 1. Interfície d'usuari (UI)

### 1.1 El botó de commutació
- S'afegirà un nou element de tipus `Button` o `Toggle` a la cantonada superior dreta de la pantalla de login.
- Contindrà una icona o text que indiqui clarament la funció: "Mode Dia" / "Mode Nit" (en català).
- Estarà accessible i no interferirà amb el formulari de login central.

### 1.2 Estats visual
- **Mode Nit (Per defecte)**: L'estat actual de l'aplicació (fons foscos, lletres daurades/blanques).
- **Mode Dia**: Fons clars (gris molt claret o blanc), lletres fosques (negre o blau fosc), mantenint l'estil de Bomberman però versió "diürna".

## 2. Comportament lògic

### 2.1 Canvi de tema
- Al prémer el botó, tota la UI del login canviarà de colors instantàniament.
- S'aplicarà una classe CSS (USS) al contenidor principal per controlar els estils de tots els fills.

### 2.2 Persistència
- La preferència de l'usuari es desarà mitjançant `PlayerPrefs` (Unity).
- Al carregar la escena de Login, el sistema consultarà si hi ha una preferència desada.
  - Si no n'hi ha, s'aplicarà el Mode Nit per defecte.
  - Si n'hi ha, s'aplicarà el tema corresponent abans de mostrar la pantalla (per evitar el flash-free).

### 2.3 Idioma
- Tots els textos afegits estaran en català:
  - "Canviar a Mode Dia"
  - "Canviar a Mode Nit"

## 3. Especificació tècnica

### 3.1 Estils (USS)
- Es definiran variables USS (si la versió d'Unity ho permet) o s'utilitzaran selectors de classe descendents:
  - `.light-mode #Contenidor`
  - `.light-mode #PanellLogin`
  - etc.

### 3.2 Codi (C#)
- Classe: `LoginController.cs`
- Nous mètodes:
    - `ToggleTheme()`: Canvia l'estat actual.
    - `ApplyTheme(bool isLightMode)`: Aplica els canvis visuals i desa la preferència.
    - `LoadTheme()`: Recupera la preferència de `PlayerPrefs`.

## 4. Casos d'ús

### CU-1: Usuari canvia a mode dia
1. L'usuari entra a la pantalla de login (veu mode nit).
2. L'usuari prem el botó "Mode Dia".
3. El fons es torna clar i el text fosc.
4. El botó canvia el seu propi text a "Mode Nit".

### CU-2: Persistència entre sessions
1. L'usuari activa el mode dia.
2. L'usuari tanca el joc.
3. L'usuari torna a obrir el joc i va a la pantalla de login.
4. La pantalla apareix directament en mode dia.
