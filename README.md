# Eudi
Eudi is framework for writing high performant multi-threaded code in Unity3D.  

Imagine if you had a lot of entities that cause a lot of impact performance, mostly because the logic script is eating a lot of the performance. It would be great if it was possible to have separate the logic and the render part, right? Then this framework is for you!  

Write all the logic in a Worker (also called as a Model or Controller), and the render in a component (also called Boss or View).
The render part will not have any performance impact that would be caused by the heavy math thingy done in the logic part.  
