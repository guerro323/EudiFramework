#Same struct reference with two arrays and unsafe.

We have this struct:
```c#
public struct TestStruct
{
  public int TestRef;
}
```

Consider this normal code:  
```c#
var testStruct = new TestStruct();
testStruct.TestRef = 16;

var arrayOne = new TestStruct[1];
arrayOne[0] = testStruct;
var newStruct = arrayOne[0];
var arrayTwo = new TestStruct[1];
arrayTwo[0] = newStruct;
newStruct.TestRef = 4;

ref var testStructFromFirstArray = ref arrayOne[0];
testStructFromFirstArray.TestRef = 32;

Debug.Log($"1: {arrayOne[0].TestRef} {arrayTwo[0].TestRef}");

newStruct.TestRef = 64;

Debug.Log($"2: {arrayOne[0].TestRef} {arrayTwo[0].TestRef}");
```

It will output:  
```
1: 32 16
2: 32 16
```

Now, let's check what will happen if we use some unsafe magic
```c#
var testStruct = new TestStruct();
testStruct.TestRef = 16;
var intptr = Marshal.AllocHGlobal(sizeof(TestStruct));
Marshal.StructureToPtr<TestStruct>(testStruct, intptr, false);

var arrayOne = new TestStruct*[1];
arrayOne[0] = (TestStruct*)intptr;
var newStruct = arrayOne[0];
var arrayTwo = new TestStruct*[1];
arrayTwo[0] = newStruct;
newStruct->TestRef = 4;

ref var testStructFromFirstArray = ref arrayOne[0];
testStructFromFirstArray->TestRef = 32;

Debug.Log($"1: {arrayOne[0]->TestRef} {arrayTwo[0]->TestRef}");

newStruct->TestRef = 64;

Debug.Log($"2: {arrayOne[0]->TestRef} {arrayTwo[0]->TestRef}");
```

This will output:
```
1: 32 32
2: 64 64
```

Unlike in the first exemple, we don't modify the value struct, we take a pointer to the struct in the stack and modify his value,  
so it's applied globally, but it's a bit risky and the 'not pointer' variable 'testStruct' don't receive the change and if we modify it,  
it doesn't apply the change on the pointers to the struct.
