# Fast IPC
Fast IPC is an open source library that supports typed messages and brings inter-process communication at a higher level for better usability.
It includes:
- Inter process communication layer using named pipes. It supports smart generation of pipe name in case of parent/child processes. Other means of communication are going to be supported in the near future
- Super fast serialization using proto-buf
- Typed event driven syntax using internally .Net built in event capability and exposing simple api such as `Subscribe` and `Publish`