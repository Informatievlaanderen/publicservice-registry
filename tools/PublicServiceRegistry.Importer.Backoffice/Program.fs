open System

let token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdF9oYXNoIjoiQUVVNTVoQ21xY0djLW9uOFAxNEgwQSIsImF1ZCI6WyI0NDRjZTdmOS0wNzBiLTQ5YWQtYjNmMi01ZWE2ZjhmYjRhNjEiLCJodHRwczovL2RpZW5zdHZlcmxlbmluZy10ZXN0LmJhc2lzcmVnaXN0ZXJzLnZsYWFuZGVyZW4iXSwiYXpwIjoiNDQ0Y2U3ZjktMDcwYi00OWFkLWIzZjItNWVhNmY4ZmI0YTYxIiwiZXhwIjoxNTUyOTkxMzI4LCJmYW1pbHlfbmFtZSI6Ik1ldHN1IiwiZ2l2ZW5fbmFtZSI6IktvZW4iLCJpYXQiOjE1NTI5ODc3NDgsImlzcyI6Imh0dHBzOi8vZGllbnN0dmVybGVuaW5nLXRlc3QuYmFzaXNyZWdpc3RlcnMudmxhYW5kZXJlbiIsIml2X2RpZW5zdHZlcmxlbmluZ19yb2xfM2QiOlsiRGllbnN0dmVybGVuaW5nc3JlZ2lzdGVyLWJlaGVlcmRlcjpPVk8wMDI5NDkiLCJEaWVuc3R2ZXJsZW5pbmdzcmVnaXN0ZXItYWRtaW46T1ZPMDAyOTQ5IiwiRGllbnN0dmVybGVuaW5nc3JlZ2lzdGVyLWNlbnRyYWxlYmVoZWVyZGVyOk9WTzAwMjk0OSIsIldlZ3dpanNCZWhlZXJkZXItb3JnYWFuYmVoZWVyZGVyOk9WTzAwMjk0OSJdLCJraWQiOiJZWDhpZGswaUpzZUQ4ZnRaOFpRYU95NU9ZOGJ4VWZQNFFZdDJORFZ3SjJrIiwic3ViIjoiY2IyN2MxY2Y0YTM5OTc2N2I1ZTM5Njk1Yjc0NjhlMGVhYWE1NWFhNyIsInZvX2RvZWxncm9lcGNvZGUiOiJHSUQiLCJ2b19kb2VsZ3JvZXBuYWFtIjoiVk8tbWVkZXdlcmtlcnMiLCJ2b19lbWFpbCI6ImtvZW4ubWV0c3VAYWdpdi5iZSIsInZvX2lkIjoiOTJjMWU5OTgtMDMwNC00YmZkLWIwMTctNmQ2NWQ0NDI2MjFkIiwidm9fb3JnY29kZSI6Ik9WTzAwMjk0OSIsInVybjpiZTp2bGFhbmRlcmVuOmRpZW5zdHZlcmxlbmluZzphY21pZCI6IjkyYzFlOTk4LTAzMDQtNGJmZC1iMDE3LTZkNjVkNDQyNjIxZCIsInVuaXF1ZV9uYW1lIjoiTWV0c3UiLCJyb2xlIjoiZGllbnN0dmVybGVuaW5nc3JlZ2lzdGVyQWRtaW4iLCJuYmYiOjE1NTI5ODc3NDh9.u_ViPQxqvceOH91GpbrcuUNEiSafmcWREfcRoXqJnt4"

let apiBaseUrl = "http://127.0.0.1:2090"
//let apiBaseUrl = "http://dienstverlening-test.basisregisters.vlaanderen:2090"
//let apiBaseUrl = "https://dienstverlening.staging-basisregisters.vlaanderen/api"

let runRegister file =
  Info.printTitle "! REGISTER SUBSIDIEREGISTER !"

  Info.printFileInfo file

  Register.getRows file
  |> Info.printRowsInfo apiBaseUrl
  |> Register.importRows apiBaseUrl token
  |> Info.printResults

let runAltNameSubsidieRegister file =
  Info.printTitle "! SET ALT LABEL SUBSIDIEREGISTER !"

  Info.printFileInfo file

  SetAltNameSubsidieRegister.getRows file
  |> Info.printRowsInfo apiBaseUrl
  |> SetAltNameSubsidieRegister.importRows apiBaseUrl token
  |> Info.printResults

let runSetCompetentAuthority file =
  Info.printTitle "! SET COMPETENT AUTHORITY !"

  Info.printFileInfo file

  SetCompetentAuthority.getRows file
  |> Info.printRowsInfo apiBaseUrl
  |> SetCompetentAuthority.importRows apiBaseUrl token
  |> Info.printResults

[<EntryPoint>]
let main _ =
    System.Console.CursorVisible <- false

    System.IO.Directory.GetFiles "Import/Register"
    |> Seq.iter runRegister

    //System.IO.Directory.GetFiles "Import/SetAltLabelSubsidieRegister"
    //|> Seq.iter runAltNameSubsidieRegister

    //System.IO.Directory.GetFiles "Import/SetCompetentAuthority"
    //|> Seq.iter runSetCompetentAuthority

    printfn ""
    printfn "Done! Press any key..."
    Console.ReadLine() |> ignore

    0
