using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UCompile;

/*
 * example of compiling a script and attaching it as a component
 * to the current game object
 * */

public class UCompileComponentTest : MonoBehaviour {

    IScript addComponentScript;

    // Use this for initialization
    void Start() {
        CSScriptEngine engine = new CSScriptEngine();
        engine.AddUsings("using UnityEngine;");
        string componentCode = @"
            public class BasicComponent : MonoBehaviour {
                void Start () { Debug.Log(""Hello, UCompile!""); }
            }
        ";
        string addComponentCode = @"
        GameObject.Find(" + "\"" + gameObject.name + "\"" +  @").AddComponent<BasicComponent>();
        ";
        System.Type componentType = engine.CompileType("BasicComponent", componentCode);
        engine.AddOnCompilationSucceededHandler(OnCompilationSuccess);
        engine.AddOnCompilationFailedHandler(OnCompilationFail);
        addComponentScript = engine.CompileCode(addComponentCode);
	}

    void OnCompilationSuccess(CompilerOutput output)
    {
        Debug.Log("compilation success");
        Debug.Log("script is null: " + (addComponentScript == null) );
    }

    void OnCompilationFail(CompilerOutput output)
    {
        Debug.Log("Fail");
        for (int i = 0; i < output.Errors.Count; i++)
            Debug.LogError(output.Errors[i]);
        for (int i = 0; i < output.Warnings.Count; i++)
            Debug.LogWarning(output.Warnings[i]);
    }

    // Update is called once per frame
    float time = 0f;
	void Update () {
		if (addComponentScript != null)
        {
            Debug.Log("Not null after " + time + " seconds");
            addComponentScript.Execute();
            addComponentScript = null;
        }
        else
        {
            time += Time.deltaTime;
        }
	}
}
