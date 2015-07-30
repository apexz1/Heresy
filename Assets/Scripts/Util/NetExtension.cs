using UnityEngine;
using System.Collections;
using System.Reflection;

public static class NetExtension {

    public static void LocalInvoke(this MonoBehaviour caller, string name, object[] args)
    {
        System.Type myType = caller.GetType();
        MethodInfo method = myType.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        if (method != null)
        {
            method.Invoke(caller, args);
        }
        else
        {
            Debug.LogWarning("NetUtil::RPC - Could not find local method " + name + " in class " + myType.Name + ". Make sure it is a public, non-static method!");
        }
    }

    public static void NetRPC(this MonoBehaviour caller, string methodName, RPCMode mode, params object[] args)
    {  
        var networkView = caller.GetComponent<NetworkView>();

        if (!Network.isClient && RPCMode.Server == mode)
        { // Local rpc call
            LocalInvoke(caller, methodName, args);
        }
        else if (Network.isClient||Network.isServer)
        {
            networkView.RPC(methodName, mode, args);
        }
        else if (mode == RPCMode.All || mode == RPCMode.AllBuffered)
        {  // If there is no network and this is supposed to go to everyone, make sure it goes to me!
            LocalInvoke(caller, methodName, args);
        }
        else
        {
            Debug.LogWarning("NetUtil::RPC - Method '" + methodName + "' on object '" + caller + "' with mode '" + mode + "' failed !" +
                " Ignoring method call!");
        }
    }

    public static void NetRPC(this MonoBehaviour caller, string methodName, NetworkPlayer target, params object[] args)
    {
        var networkView = caller.GetComponent<NetworkView>();
        //Debug.LogWarning("NetUtil::RPC - Method '" + target);
        if (target == null || target.ToString()=="0")
        { // Local rpc call
            LocalInvoke(caller, methodName, args);
        }
        else if (Network.isClient || Network.isServer)
        {
            networkView.RPC(methodName, target, args);
        }
        else
        {
            Debug.LogWarning("NetUtil::RPC - Method '" + methodName + "' on object '" + caller + "' with target '" + target + "' failed !" +
                " Ignoring method call!");
        }
    }
}
