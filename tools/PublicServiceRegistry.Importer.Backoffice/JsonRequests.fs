module PublicService.JsonRequests

  open FSharp.Data

  [<Literal>]
  let newPublicServiceSample = """
  {
    "naam": "Mijn dienstverlening"
  }
  """

  type NewPublicService = JsonProvider<newPublicServiceSample, RootName = "Create">

  [<Literal>]
  let updatePublicServiceSample = """
  {
    "id": "Mijn id",
    "naam": "Mijn naam",
    "bevoegdeAutoriteitOvoNummer": "Mijn bevoegdeAutoriteitOvoNummer",
    "isSubsidie": "true"
  }
  """

  type UpdatePublicService = JsonProvider<updatePublicServiceSample, RootName = "Update">

  [<Literal>]
  let updateAlternativeNameIpdcSample = """
  {
    "labels": {
      "Ipdc": "Mijn Alternatieve naam"
    }
  }
  """

  type UpdateAlternativeNameIpdc = JsonProvider<updateAlternativeNameIpdcSample, RootName = "UpdateAlternativeNameIpdc">

  [<Literal>]
  let updateAlternativeNameSubsidieRegisterSample = """
  {
    "labels": {
      "Subsidieregister": "Mijn Alternatieve naam"
    }
  }
  """

  type UpdateAlternativeNameSubsidieRegister = JsonProvider<updateAlternativeNameSubsidieRegisterSample, RootName = "UpdateAlternativeNameSubsidieRegister">
