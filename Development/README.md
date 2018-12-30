# Development
For easier development of the opsi rpc interfaces you can patch your opsi
installation to print all rpc queries. So you're able to send requests using
the original opsi-configed client and rebuild them in this project.

```
patch -p1 < worker.patch
```
