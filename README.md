# JsonPi
A Pi Calculus interpreter.

## Purpose
To aid in learning the pi calculus and experiment with the pi calculus model of computation. To learn about the pi calculus see https://en.wikipedia.org/wiki/Π-calculus.

## Description
JsonPi is designed to interpret programs based on the pi calculus as described in chapter one of the book: "The pi-calculus: a Theory of Mobile Processes" by Davide Sangiorgi and David Walker. 
The interpreter is currently implemented in F# but designed to be easily ported to other languages. JsonPi programs are represented in JSON so are meant to be easily executed on any platform which supports an interpreter.

## Primary features
* Designed to run pi calculus programs as defined in the book "The pi-calculus: a Theory of Mobile Processes"
* Represents names as any arbitrary JSON
* Modularity for building reusable libraries of pi calculus components
* Extensibility to the backing language of the interpreter (right now F#)

## Example
  world\<hello\>; | world(message);
  
Above is an example of the most basic and fundamental capability of the pi calculus and JsonPi, the comm rule. 
* The identifier 'world' is a pi calculus name which, in this case, is being used as a channel of communication used by two processes, one sending and one receiving
* The identifier 'hello' is a pi calculus name which is used here as data being sent over the channel 'world'
* The identifier 'message' is also a pi calculus name used as a handle to whatever data is being received over the channel 'world'
* The construct channel\<name\> is read "Send name on channel"
* The construct channel(name) is read "Receive name on channel"
* The semi-colon ';', is the inactive process which terminates a process
* The vertical bar '|', separates two processes running in parallel. (JsonPi is single threaded but more on that later.)

The constructs of send and receive ('channel\<name\>' and 'channel(name)') are called prefixes which can be strung together into a series of consecutive steps. 
For any process, execution is halted on a prefix until a matching send or receive operation is pending on another process. 
So, in the example above, a matching send and receive operation is present on the channel 'world' and so execution will progress one more step after the send and receive operations. 
In this case, the next step for both processes is the inactive process ';', which completes execution.

## Elements of the JsonPi Language
### Send on a channel
#### Syntax 
  channel '\<' name [ ',' name] '>'
#### Example
  x\<a,b,c\>
#### Description
The names 'a', 'b' and 'c' are being sent on the channel 'x'. 
Execution will block for this process until another process is attempting to receive on channel 'x'. 
All other processes are free to execute.

### Receive on a channel
#### Syntax
  channel '(' name [ ',' name] ')'
#### Example
  x(f,g,h)
#### Description.
The names 'f', 'g' and 'h' are used as placeholders for data to be received over the channel 'x'. 
Execution will block for this process until another process is attempting to send on channel 'x'. 
All other processes are free to execute.

### Run processes in parallel
#### Syntax
  P '|' Q
#### Example
  x\<a,b,c\>; | x(f,g,h);
#### Description
Two processes are running in parallel. One is sending on the channel 'x' and the other is receiving on the channel 'x'. The three names of 'a', 'b' and 'c' will be sent over channel 'x' and then both processes with become inactive.
JsonPi is single threaded but, conceptually, the sending process on the left and the receiving process on the right should be thought of as running at the same time. 
The ending ';' is important and required in both the left and right process because prefixes must be followed by a process.
In this case the inactive process.

### Create a New Name
#### Syntax
  'new' '(' name [ ':' name-type ] ')' [ '=' JSON-Data ]
### Examples
  new (x)
  
  new (x:MyNameType)
  
  new (x) = { "City" : "Seattle" }
  
  new (x:MyNameType) = { "City" : "Seattle" }
#### Description
The 'new' syntax is used to create a restricted name. 
Names in JsonPi do not have to be restricted, you can use any name you like. 
However, restricting a name ensures that the name is not one that has been used before, even though it might share an identifier. 
For example:
  x\<a\>; | new (x) x(f);
will remained blocked with the send operation on the channel identified by 'x' and the receive operation identified by the new channel identified by 'x' as happening on separate channels.
Restricted names can be sent outside the scope of 'new'. For example:
  new (a) x\<a\>; | x(f) ( f\<b\>; | f(c); )
Here, the new name 'a' is being sent over the channel 'x' and then used as a channel over which the name 'b' is being sent. 
This demonstrates how restricted names can be extruded across processes.

Restricted names are also the way to indicate to JsonPi that the name has an optional type and an optional JSON data object. 
The name-type is used to map to an extensibility capability provided by the backing language of the interpreter (more below).
The JSON-Data object is used carried as additional data for the name. Any arbitrary JSON data is acceptable.

### Repeated processes
#### Syntax
  '!' P
#### Example
  !(x(f);)
#### Description
The execution of processes moves from prefix to prefix until reaching the inactive process which completes the execution.
To keep a process active the replication systax is used. After each successful step in the process defined under a replication a new, parallel process is started from the beginning. 
For example:
  x\<a\>; | x\<b\> y(c); | ( !(x(d) y\<d\>;) )
defines three processes. 
The first, from the left, will attempt to send 'a' on 'x' and then become inactive. 
The second will attempt to send 'b' on 'x' and then receive 'c' on 'y'.
The third process uses replication to continuously receive 'd' on 'x' and then send it on 'y'.
When the replication executes the first receive on 'x', it will start two processes in parallel, one which contniues the remainder of the first execution, that is, sending 'd' on 'y'. 
And the other to begin the process from the start by attempting to receive 'd' on 'x' again.
Of these three processes, the first two will run to completion but the replication will retain one process awaiting a receive on 'x'.

### Conditional Execution
#### Syntax
  '[' name '=' ']' Prefix
#### Examples
  x(a) [a=b]y\<c\>; | x\<b\>;

  x(a) y(c) [a=b][c=d]z(f); | x\<b\> y\<d\> z\<g\>;
#### Description
The match syntax defines a conditional guard on a prefix. The prefix after the match expression will not only block on its operation (sending or receiving) but first the condition in brackets must be met.
In the first example, the name 'b' is sent over the channel 'x' and received as 'a'. 
Once 'a' is received as 'b', subsequent values of 'a' are substituted with 'b' which would now evaluate to [b=b] which is true.
This would then allow the attempt to send 'c' on 'y'.
The second example demonstrates that multiple match expressions can be used before a send or receive prefix.

### Branching
#### Syntax
  'choose' 'when' Prefix 'then' Process [...] [ 'default' Process ] 'end'
#### Example
  choose
    when x(a) then ;
    when y(b) then z\<b\>;
    when [c=d]x\<e\> then y(f);
    default z(g);
  end
#### Description
The 'choose' syntax is used to define a pi calculus sum expression. The prefix of each 'when' clause is evaluated in order from first to last. 
If the prefix does not block then it will proceed to the process of the 'then' clause and the other 'when' clauses and an optional 'default' will be abandoned.
The optional default process is executed if all of the 'when' clause prefixes are blocked.

### Process Binding
#### Syntax
  'let' Identifier '=' Process
#### Examples
  let P = x\<a\>;
  let Q = x(b);
#### Description
Process bindings give an identifier to a process for the sake of code organization and reuse.

### Process Reference
#### Syntax
  '$' Identifier
#### Examples
  $P | $Q
  
  x\<a\> $P | x(b) $Q
#### Description
Processes defined with a previous process binding can be used with a process reference. A process reference can be used anywhere a process could also be used.

### Module Definition
#### Syntax
  'module' Identifier
#### Example
  module MyModule
#### Description
Modules provide a way to group related code together. All pi calculus code following a 'module' definition is combined within that module. 
Modules can define process bindings which can be used by other programs which reference the defining module.

### Module Reference
#### Syntax
  'using' Identifier
#### Example
  using MyModule
#### Description
Referencing a module makes all of the top-level process bindings of the defining module available to the program. 
Modules also have a base process which executes in parallel with the process which references the module.

## Extensibility
JsonPi provides a mechanism to execute code written in the language of the interpreter (currently only F#).
This allows for JsonPi programs to use the all the features of the backing language.
While F# is the only backing language today, other backing languages like JavaScript could provide an interpreter with the ability to call scripts from a pi calculus program.
There are three events when external code can be called:
* On the creation of a new name
* During a send operation
* During a receive operation
When the external code is executed it can optionally return a pi calculus process which will executed after the external code.
The name type of the channel is used as a map to the external functionality.
For an example of the extensibility mechanism, see the basic tests in the Test project.

## Threading
Even though pi calculus is meant for writing concurrent processes, the JsonPi interpreter executes sequentially.
This was done intentionally to simplify the implementation and to provide a repeatable execution to aid in understanding and learning the pi calculus.
There is some chance that the PiNamespace will be made thread safe and exposed to be shared across interpreter instances which would provide for multiple threads of execution but that is not yet complete.


