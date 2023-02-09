 <article class="content wrap" id="_content" data-uid="Global Namespace.AddressablesPlayerBuildProcessor">
  
  
  <h1 id="Global_Namespace_AddressablesPlayerBuildProcessor" data-uid="Global Namespace.AddressablesPlayerBuildProcessor">Class AddressablesPlayerBuildProcessor
  </h1>
  <div class="markdown level0 summary"><p>Maintains Addresssables build data when processing a player build.</p>
</div>
  <div class="markdown level0 conceptual"></div>
  <div class="inheritance collapsible show-bottom">
    <h5>Inheritance</h5>
    <div class="level0"><a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object">Object</a></div>
    <div class="level1"><span class="xref">AddressablesPlayerBuildProcessor</span></div>
  </div>
  <div class="inheritedMembers collapsible">
    <h5>Inherited Members</h5>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString">Object.ToString()</a>
    </div>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_">Object.Equals(Object)</a>
    </div>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_System_Object_">Object.Equals(Object, Object)</a>
    </div>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.referenceequals#System_Object_ReferenceEquals_System_Object_System_Object_">Object.ReferenceEquals(Object, Object)</a>
    </div>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode">Object.GetHashCode()</a>
    </div>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType">Object.GetType()</a>
    </div>
    <div>
      <a class="xref" href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone">Object.MemberwiseClone()</a>
    </div>
  </div>
  <h6><strong>Namespace</strong>: <a class="xref" href="Global%20Namespace.html">Global Namespace</a></h6>
  <!--h6><strong>Assembly</strong>: solution.dll</h6-->
  <h5 id="Global_Namespace_AddressablesPlayerBuildProcessor_syntax">Syntax</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">public class AddressablesPlayerBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport, IOrderedCallback</code></pre>
  </div>
  <h3 id="properties">Properties
  </h3>
  
  
  <a id="Global_Namespace_AddressablesPlayerBuildProcessor_callbackOrder_" data-uid="Global Namespace.AddressablesPlayerBuildProcessor.callbackOrder*"></a>
  <h4 id="Global_Namespace_AddressablesPlayerBuildProcessor_callbackOrder" data-uid="Global Namespace.AddressablesPlayerBuildProcessor.callbackOrder">callbackOrder</h4>
  <div class="markdown level1 summary"><p>Returns the player build processor callback order.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="decalaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">public int callbackOrder { get; }</code></pre>
  </div>
  <h5 class="propertyValue">Property Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><a class="xref" href="https://docs.microsoft.com/dotnet/api/system.int32">Int32</a></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <h5 class="implements">Implements</h5>
      <div><a class="xref" href="https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Build.IOrderedCallback-callbackOrder.html">IOrderedCallback.callbackOrder</a></div>
  <h3 id="methods">Methods
  </h3>
  
  
  <a id="Global_Namespace_AddressablesPlayerBuildProcessor_OnPostprocessBuild_" data-uid="Global Namespace.AddressablesPlayerBuildProcessor.OnPostprocessBuild*"></a>
  <h4 id="Global_Namespace_AddressablesPlayerBuildProcessor_OnPostprocessBuild_UnityEditor_Build_Reporting_BuildReport_" data-uid="Global Namespace.AddressablesPlayerBuildProcessor.OnPostprocessBuild(UnityEditor.Build.Reporting.BuildReport)">OnPostprocessBuild(BuildReport)</h4>
  <div class="markdown level1 summary"><p>Removes temporary data created as part of a build.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="decalaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">public void OnPostprocessBuild(BuildReport report)</code></pre>
  </div>
  <h5 class="parameters">Parameters</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Name</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><a class="xref" href="https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Build.Reporting.BuildReport.html">BuildReport</a></td>
        <td><span class="parametername">report</span></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <h5 class="implements">Implements</h5>
      <div><a class="xref" href="https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Build.IPostprocessBuildWithReport.OnPostprocessBuild.html">IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport)</a></div>
  
  
  <a id="Global_Namespace_AddressablesPlayerBuildProcessor_OnPreprocessBuild_" data-uid="Global Namespace.AddressablesPlayerBuildProcessor.OnPreprocessBuild*"></a>
  <h4 id="Global_Namespace_AddressablesPlayerBuildProcessor_OnPreprocessBuild_UnityEditor_Build_Reporting_BuildReport_" data-uid="Global Namespace.AddressablesPlayerBuildProcessor.OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport)">OnPreprocessBuild(BuildReport)</h4>
  <div class="markdown level1 summary"><p>Initializes temporary build data.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="decalaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">public void OnPreprocessBuild(BuildReport report)</code></pre>
  </div>
  <h5 class="parameters">Parameters</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Name</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><a class="xref" href="https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Build.Reporting.BuildReport.html">BuildReport</a></td>
        <td><span class="parametername">report</span></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <h5 class="implements">Implements</h5>
      <div><a class="xref" href="https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Build.IPreprocessBuildWithReport.OnPreprocessBuild.html">IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport)</a></div>
</article>
          </div>

