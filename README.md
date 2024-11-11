# MocartShelf
Unity Version is 2022.3.18f1

Libraries used:

#UniRX - https://github.com/neuecc/UniRx
Allows reactive event-driven code.
Note: there is a newer version of it Called R3, but I have yet to use it and thus decided to go with something familiar and reliable.

#WebGLInput - https://github.com/kou-yeung/WebGLInput
For the ability to use a mobile device keyboard in webGL. My prior experience to this assignment with WebGL was a game jam.
I was not aware that mobile input for webGL isn't trivial, this is the best solution I found in the time I had left.

#.Net HttpClient
I opted to use .Net's http client for web requests. This is because it allows better control of API request life-cycle
In addition, it allows running these requests in async interdependently on Unity's life-cycle and lives outside the coroutines.
Unfortunately, I had to scrap the whole thing last minute because it uses System.Threading and that isn't supported in WebGL.

I tried following the SOLID principles and keeping things tidy.
