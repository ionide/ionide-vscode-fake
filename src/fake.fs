[<ReflectedDefinition>]
module Ionide.VSCode

open System
open System.Text.RegularExpressions
open FunScript
open FunScript.TypeScript
open FunScript.TypeScript.fs
open FunScript.TypeScript.child_process
open FunScript.TypeScript.vscode

type Fake() =
    member x.activate(state:obj) =
        let t = workspace.Globals.getPath ()
        commands.Globals.registerCommand("fake.sayHello", (fun _ ->
            window.Globals.showInformationMessage "Hello world!") |> unbox<CommandCallback>
            ) |> ignore
        ()
