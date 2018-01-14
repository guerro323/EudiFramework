This scene and his scripts replicate the script shown in the Unite 2017
https://gist.github.com/valyard/e96060ee8e5a9d52d43901c7c6ad4288 (slides)

Included scene:
- You can find the cubes with the "RotationSpeed" component/module in the "Dynamic" tab of the scene hierarchy.
- You can find the system in "Meta" tab of the scene hierarchy

Some modifications from the original one:
- In systems: MatchEntities return all entities with the components marked with [InjectTuples]
- EudiMatchArray<> don't contains any variables unlike ComponentDataArray<>, it's just to know if we hold multiple components
- For getting the components in a job, you first get the entity, then you get the module from the entity (EudiEntity.GetLazyModule<>).
- Components are called modules in EUDI.
- An entity system is a world component.