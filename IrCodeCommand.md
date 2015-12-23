# ir\_code (command) #
## Summary ##

The ir\_code command emulates a key press on a remote control.

This command takes one parameter: `ir_code`.

The value for this parameter is the hexidecimal representation of a 32-bit code, as specified by the NEC protocol, in little endian notation (`*`).

A NEC code is made up of a customer code (16 bits), an infrared code (8 bits), and the inverse of the infrared code (8 bits). The last 2 bytes always add up to `FF` (e.g. `00 + FF`, `01 + FE`), allowing for up to 255 unique key code combination.

The customer code used across all supported keys is `BF 00`.

(`*`): bytes must be transmitted in reverse order. The Dune's CPU uses little-endian byte sorting, so the least significant (rightmost) byte should be the first in the sequence, followed by the other bytes.


## Syntax ##

| **Parameter**         | **Value(s)** | **Example**        | **API Version(s)** |
|:----------------------|:-------------|:-------------------|:-------------------|
| [cmd](Cmd.md)           | ir\_code     | cmd=ir\_code       | ≥ 1                |
| [ir\_code](IrCode.md)    | {code}       | ir\_code=EB14BF00  | ≥ 1                |
| [[timeout](Timeout.md)] | {n}          | timeout=20         | ≥ 1                |

## Examples ##

Send the 'ENTER' key:

`http://dune/cgi-bin/do?cmd=ir_code&ir_code=EB14BF00`

`EB14BF00` is the little-endian notation of the actual NEC code: `00BF14EB`