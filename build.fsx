#r "paket:
version 5.241.6
framework: netstandard20
source https://api.nuget.org/v3/index.json
nuget Be.Vlaanderen.Basisregisters.Build.Pipeline 3.3.1 //"

#load "packages/Be.Vlaanderen.Basisregisters.Build.Pipeline/Content/build-generic.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.JavaScript
open ``Build-generic``

// The buildserver passes in `BITBUCKET_BUILD_NUMBER` as an integer to version the results
// and `BUILD_DOCKER_REGISTRY` to point to a Docker registry to push the resulting Docker images.

// NpmInstall
// Run an `npm install` to setup Commitizen and Semantic Release.

// DotNetCli
// Checks if the requested .NET Core SDK and runtime version defined in global.json are available.
// We are pedantic about these being the exact versions to have identical builds everywhere.

// Clean
// Make sure we have a clean build directory to start with.

// Restore
// Restore dependencies for debian.8-x64 and win10-x64 using dotnet restore and Paket.

// Build
// Builds the solution in Release mode with the .NET Core SDK and runtime specified in global.json
// It builds it platform-neutral, debian.8-x64 and win10-x64 version.

// Test
// Runs `dotnet test` against the test projects.

// Publish
// Runs a `dotnet publish` for the debian.8-x64 and win10-x64 version as a self-contained application.
// It does this using the Release configuration.

// Pack
// Packs the solution using Paket in Release mode and places the result in the dist folder.
// This is usually used to build documentation NuGet packages.

// Containerize
// Executes a `docker build` to package the application as a docker image. It does not use a Docker cache.
// The result is tagged as latest and with the current version number.

// DockerLogin
// Executes `ci-docker-login.sh`, which does an aws ecr login to login to Amazon Elastic Container Registry.
// This uses the local aws settings, make sure they are working!

// Push
// Executes `docker push` to push the built images to the registry.

let product = "Basisregisters Vlaanderen"
let copyright = "Copyright (c) Vlaamse overheid"
let company = "Vlaamse overheid"

let dockerRepository = "public-service-registry"
let assemblyVersionNumber = (sprintf "2.%s")
let nugetVersionNumber = (sprintf "%s")

let build = buildSolution assemblyVersionNumber
let setVersions = (setSolutionVersions assemblyVersionNumber product copyright company)
let test = testSolution
let publish = publish assemblyVersionNumber
let pack = pack nugetVersionNumber
let containerize = containerize dockerRepository
let push = push dockerRepository

Target.create "CleanAll" (fun _ ->
  Shell.cleanDir buildDir
  Shell.cleanDir ("src" @@ "PublicServiceRegistry.UI" @@ "wwwroot")
)

// Solution -----------------------------------------------------------------------

Target.create "Restore_Solution" (fun _ -> restore "PublicServiceRegistry")

Target.create "Build_Solution" (fun _ ->
  setVersions "SolutionInfo.cs"
  build "PublicServiceRegistry")

Target.create "Site_Build" (fun _ ->
  Npm.exec "build" id

  let dist = (buildDir @@ "PublicServiceRegistry.UI" @@ "linux")
  let source = "src" @@ "PublicServiceRegistry.UI"

  Shell.copyDir (dist @@ "wwwroot") (source @@ "wwwroot") (fun _ -> true)
  Shell.copyFile dist (source @@ "Dockerfile")
  Shell.copyFile dist (source @@ "default.conf")
  Shell.copyFile dist (source @@ "config.js")
  Shell.copyFile dist (source @@ "init.sh")
)

Target.create "Test_Solution" (fun _ -> test "PublicServiceRegistry")

Target.create "Publish_Solution" (fun _ ->
  [
    "PublicServiceRegistry.Api.Backoffice"
    "PublicServiceRegistry.Projector"
    "PublicServiceRegistry.Projections.Backoffice"
    "PublicServiceRegistry.OrafinUpload"
  ] |> List.iter publish)

Target.create "Pack_Solution" (fun _ ->
  [
    "PublicServiceRegistry.Api.Backoffice"
  ] |> List.iter pack)

Target.create "Containerize_ApiBackoffice" (fun _ -> containerize "PublicServiceRegistry.Api.Backoffice" "api")
Target.create "PushContainer_ApiBackoffice" (fun _ -> push "api")

Target.create "Containerize_Projections" (fun _ -> containerize "PublicServiceRegistry.Projector" "projector")
Target.create "PushContainer_Projections" (fun _ -> push "projector")

Target.create "Containerize_OrafinUpload" (fun _ -> containerize "PublicServiceRegistry.OrafinUpload" "batch-orafin")
Target.create "PushContainer_OrafinUpload" (fun _ -> push "batch-orafin")

Target.create "Containerize_Site" (fun _ -> containerize "PublicServiceRegistry.UI" "ui")
Target.create "PushContainer_Site" (fun _ -> push "ui")

// --------------------------------------------------------------------------------

Target.create "Build" ignore
Target.create "Test" ignore
Target.create "Publish" ignore
Target.create "Pack" ignore
Target.create "Containerize" ignore
Target.create "Push" ignore

"NpmInstall"
 // ==> "DotNetCli"
  ==> "CleanAll"
  ==> "Restore_Solution"
  ==> "Build_Solution"
  ==> "Build"

"Build"
  ==> "Site_Build"
  ==> "Test_Solution"
  ==> "Test"

"Test"
  ==> "Publish_Solution"
  ==> "Publish"

"Publish"
  ==> "Pack_Solution"
  ==> "Pack"

"Pack"
  ==> "Containerize_ApiBackoffice"
  ==> "Containerize_Projections"
  ==> "Containerize_OrafinUpload"
  ==> "Containerize_Site"
  ==> "Containerize"
// Possibly add more projects to containerize here

"Containerize"
  ==> "DockerLogin"
  ==> "PushContainer_ApiBackoffice"
  ==> "PushContainer_Projections"
  ==> "PushContainer_OrafinUpload"
  ==> "PushContainer_Site"
  ==> "Push"
// Possibly add more projects to push here

// By default we build & test
Target.runOrDefault "Test"
