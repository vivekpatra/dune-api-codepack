# timeout #

## Summary ##

`timeout` indicates the amount of seconds before the HTTP server should return a timeout and close the connection.

A connection remains open until command execution completes, or until the timeout is reached.

## Values ##

|           | **Timeout** |
|:----------|:------------|
| **Minimum** | `1`         |
| **Maximum** | `∞`         |
| **Default** | `20`        |

## Notes ##

When the connection is closed due to a timeout, command execution is **not** aborted.