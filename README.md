# Eudi

Eudi is a framework for writing high performant multi-threaded code in Unity3D.  

Imagine if you had a lot of entities that cause a lot of impact performance, mostly because the logic script is eating a lot of the performance. It would be great if it was possible to have separate the logic and the render part, right? Then this framework is for you!  

Write all the logic in a Worker (also called as a Model or Controller), and the render in a component (also called Boss or View).
The render part will not have any performance impact that would be caused by the heavy math thingy done in the logic part.  

## Synchronization type

***

The framework give two types for the synchronization type for workers threads:
* Unity synchronization type: give access to Unity types but give a big impact on performance compared to the true multithreading, choose this if you want a worker that use Unity methods each frame.
* True MultiTrheading synchronization type: more for performance but it don't give any access to the main-thread only Unity types, choose this if you want a worker that do **very very heavy** task.

## Replica time

***

The framework give the possibility to have the same variables of Unity `Time` class for the 'true multithreaded' workers

## Benchmark

***

I've run some benchmark based on the scene of the first exemple (`E1StressTestCubeMovement`):  
Properties:  
* 10 000 cubes
* 1000 iteration for the heavy rotation work
* 6ms refresh rate for workers

**Behaviour: Unity Standard, Synchronization Type: Don't work on 'Unity Standard'**
~380ms
![that an image](https://raw.githubusercontent.com/guerro323/EudiFramework/master/Images/BenchmarkE1_unitystandard_unity.png)  

**Behaviour: Eudi Actor Behaviour, Synchronization Type: Unity**
~155ms
![that an image](https://raw.githubusercontent.com/guerro323/EudiFramework/master/Images/BenchmarkE1_eudi_unity.png)  

**Behaviour: Eudi Actor Behaviour, Synchronization Type: True multithreading**
~10ms
![that an image](https://raw.githubusercontent.com/guerro323/EudiFramework/master/Images/BenchmarkE1_eudi_truethreading.png)
