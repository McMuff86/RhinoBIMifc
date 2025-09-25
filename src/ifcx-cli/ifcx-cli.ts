import * as process from "process";
import * as fs from "fs";

import { components } from "../../schema/out/ts/ifcx";
import { Diff, Federate } from "../ifcx-core/workflows";
import { ExampleFile } from "../test/example-file";
import { SchemasToOpenAPI } from "../ifcx-core/schema/schema-export";
import { spawnSync } from "child_process";
import * as path from "path";

type IfcxFile = components["schemas"]["IfcxFile"];

let args = process.argv.slice(2);

console.log("running ifcx [alpha]...", JSON.stringify(args));
console.log();


function processArgs(args: string[])
{
    let operation = args[0];

    if (operation === "schema_to_openapi")
    {
        let path = args[1];
        if (!path.endsWith(".ifcx.json")) throw new Error(`Expected extension .ifcx.json`);
        let file = JSON.parse(fs.readFileSync(path).toString());
        let openAPI = SchemasToOpenAPI(file as IfcxFile);
        let openaAPIPath = path.replace(".ifcx.json", ".openapi.yml");
        fs.writeFileSync(openaAPIPath, openAPI);
    }
    else if (operation === "diff" || operation === "federate")
    {
        if (args.length !== 4) throw new Error(`expected 3 arguments`);

        let path1 = args[1];
        let path2 = args[2];
        let outputPath = args[3];

        let data1 = JSON.parse(fs.readFileSync(path1).toString());
        let data2 = JSON.parse(fs.readFileSync(path2).toString());

        let result: IfcxFile | null = null;
        if (operation === "diff")
        {
            result = Diff(data1, data2);
        }
        if (operation === "federate")
        {
            result = Federate([data1, data2]);
        }

        fs.writeFileSync(outputPath, JSON.stringify(result, null, 4));
    }
    else if (operation === "make_default_file")
    {
        let path = args[1];
        fs.writeFileSync(`${path}.ifcx.json`, JSON.stringify(ExampleFile(), null, 4))
    }
    else if (operation === "convert_ifc_to_ifcx")
    {
        if (args.length < 3) throw new Error(`usage: convert_ifc_to_ifcx <in.ifc> <out.ifcx>`);
        const inPath = args[1];
        const outPath = args[2];
        if (!fs.existsSync(inPath)) throw new Error(`file not found: ${inPath}`);
        // Pipeline: ifc -> usda -> ifcx using python utilities with uv
        const tmpUsd = outPath.replace(/\.ifcx$/i, ".usda");
        const script1 = path.resolve(__dirname, "utils/python/ifc4-to-usda.py");
        const script2 = path.resolve(__dirname, "utils/python/usda-to-json.py");
        if (!fs.existsSync(script1)) throw new Error(`missing script: ${script1}`);
        if (!fs.existsSync(script2)) throw new Error(`missing script: ${script2}`);
        const uv = process.platform === 'win32' ? 'uv' : 'uv';
        const step1 = spawnSync(uv, ["run", "python", script1, inPath, tmpUsd], { stdio: "inherit", shell: true, cwd: path.resolve(__dirname, "..") });
        if (step1.status !== 0) throw new Error("ifc4-to-usda failed");
        const step2 = spawnSync(uv, ["run", "python", script2, tmpUsd, outPath], { stdio: "inherit", shell: true, cwd: path.resolve(__dirname, "..") });
        if (step2.status !== 0) throw new Error("usda-to-json failed");
        try { fs.unlinkSync(tmpUsd); } catch {}
        console.log(`wrote ${outPath}`);
    }
    else if (!operation || operation === "help")
    {
        console.log(`available commands:`);
        console.log(`schema_to_openapi`);
        console.log(`diff`);
        console.log(`federate`);
        console.log(`make_default_file`);
        console.log(`convert_ifc_to_ifcx`);
        console.log(`help`);
    }
    else
    {
        console.log(`Unknown command ${operation}`)
    }
}

processArgs(args);