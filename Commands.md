# Commands #
## Summary ##
A command is sent over HTTP using simple HTTP request. As you would expect from any HTTP API, the target device is specified in the address portion. The target process (IP Control) is specified as the path. The command instructions are specified in the [query string](http://en.wikipedia.org/wiki/Query_string) portion of the request. Both [GET and POST](http://en.wikipedia.org/wiki/HTTP_GET#Request_methods) methods are supported (with limitations: see notes).

For the sake of simplicity, this documentation assumes the GET method (see [Final Notes](ApiReference#Final_Notes.md)).

## Syntax ##

| **Complete example** | http://10.0.0.1:80/cgi-bin/do?cmd=status&timeout=20 |
|:---------------------|:----------------------------------------------------|

The following table breaks the command syntax into its core components.

|             | **Protocol** |  **Address**                       | [**Port**]      | **Path**      | **Query**                         |
|:------------|:-------------|:-----------------------------------|:----------------|:--------------|:----------------------------------|
| **Value(s)**  | `http://`    | `{IP address}` -or- `{DNS name}`   | `:1 .. 65535`   | /cgi-bin/do   | ?`name`=`value`[& ... [& ... ]]   |
| **Example**   | `http://`    | 10.0.0.1                           | `:80`           | /cgi-bin/do   | ?cmd=status&timeout=20            |

  * **Protocol**: the API communicates over the HTTP protocol.
  * **Address**: IP addresses and DNS names are acceptable.
  * **Port**: optional parameter, separated from the address by a single colon ("`:`"). Defaults to port 80 in most implementations.
  * **Path**: the relative path to the target process.
  * **Query**: one or more name/value pairs, separated from one another by an ampersand. A question mark indicates the start of the query portion.


## Values ##

From this point forward, we'll be focusing only on the query portion of command requests.

If available, clicking on a parameter will take you to a page with all known values.

| **Parameter**         | **Value(s)**  | **Example**  | **Summary**                                                    |
|:----------------------|:--------------|:-------------|:---------------------------------------------------------------|
| [cmd](Cmd.md)           | `{command}`   | cmd=status   | Indicates what action to perform.                              |
| [[timeout](Timeout.md)] | `{n}`         | timeout=20   | Indicates the amount of seconds before a timeout is reached.   |

## Notes ##

  * For POST requests, the `Content-Type` header must be set to `application/x-www-form-urlencoded`.
  * Current versions report that a `multipart/form-data` content-type is recognized but not implemented.
  * Some web browsers enforce that the `Content-Type` header also specifies a charset. In current versions, this breaks the API.