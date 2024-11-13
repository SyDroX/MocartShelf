# MocartShelf
[Link to Playable](https://sydrox.itch.io/mocart)

[Link to Playable with forced landscape for mobile](https://sydrox.itch.io/mocartforcelandscape)

Built with Unity version 2022.3.18f1

# Overall design:
The scripts' folder structure is based on the logic of what they do.
The scene is divided into 3 parts, the scene itself, the UI, and the logical product-related scripts.
I took the liberty to illustrate the products using fruits and vegetables to give the result a bit more life :)

# Libraries used:

## UniRX - https://github.com/neuecc/UniRx
Allows reactive event-driven code. Decouples code and no need to reference a bunch of MonoBehaviours to one another.
All data transfer between Monobehaviours is done via Event Arguments (EventData folder).  
Note: There is a newer version Called [Cysharp/R3](https://github.com/Cysharp/R3), however, it doesn't support  WebGL.

## WebGLInput - https://github.com/kou-yeung/WebGLInput
Allows the use of a native keyboard on a mobile device in webGL. This is the best working solution I found (tested on Android, iPhone and iPad).
[This](https://discussions.unity.com/t/input-field-not-working-in-webgl-build-in-mobile-browser/717658) is the most comprehensive discussion I found about the matter.
