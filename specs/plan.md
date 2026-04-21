# Plan — Estratègia d'implementació

Aquest pla detalla els passos tècnics per implementar el commutador de tema seguint l'especificació.

## Fase 1: Preparació dels Estils (USS)
- Modificar `Assets/USS/LoginStyles.uss`.
- Afegir una classe global `.tema-clau` (o `.light-mode`) que sobreescrigui els colors del contenidor, panell, títol i botons.
- Utilitzar colors amb bon contrast:
    - Fons: `#F0F0F0` (Gris clar)
    - Panell: `#FFFFFF` (Blanc)
    - Text: `#12121E` (Fosc)

## Fase 2: Modificació de la UI (UXML)
- Obrir `Assets/UXML/LoginScreen.uxml`.
- Afegir un `Button` amb `name="BotoTema"` a dins del `Contenidor` però fora del `PanellLogin` si es vol una posició absoluta, o dins per simplicitat.
- Estilitzar el botó per a que sembli un toggle o tingui una mida discreta.

## Fase 3: Lògica a C# (LoginController)
- Actualitzar `LoginController.cs`:
    - Definir constant de `PlayerPrefs` per al tema.
    - Capturar la referència a `BotoTema` i al `Contenidor` (root).
    - Implementar el mètode `AlternarTema()`.
    - Al `Start()`, cridar a una funció que comprobi el `PlayerPrefs` i apliqui el tema guardat.

## Fase 4: Verificació i Poliment
- Comprovar que els textos estiguin en català.
- Verificar que la transició no generi parpelleigs estranys.
- Verificar que en tancar i obrir l'escena es manté la preferència.

## Riscos i Mitigació
- **Risc**: Unity UI Toolkit no aplica els canvis de classe instantàniament en algunes versions.
- **Mitigació**: Utilitzar `RemoveFromClassList` i `AddToClassList` seguit de `MarkDirtyRepaint()` si cal.
