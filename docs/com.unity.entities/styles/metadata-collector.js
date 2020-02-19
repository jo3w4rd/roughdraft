let splitPathname  = location.pathname.split('/');
let hasPopulated = false;
packageName = splitPathname[2].split('@')[0];

const versionSwitcherHtml = `
<div id="version-switcher-select">
    <div class="component-select">
        <div id="component-select-current-display" class="component-select__current">
        ` + thisPackageMetaData.displayTitle + `
        </div>

        <ul id="version-switcher-ul" class="component-select__options-container">
        </ul>
    </div>
</div>
`;

function getPackageMetaData(callback){
    if (location.hostname === "localhost"){
        packageName = splitPathname[1].split('@')[0];
    }
    
    let requestURL = "../../metadata/" + packageName + "/metadata.json";
    
    request(requestURL, callback);
}

function request(requestURL, callback){
    if (!hasPopulated){
        $.getJSON(requestURL, function(data){
            console.log("Getting meta data...");
            callback(data);
        }).fail(function(){
            console.log("No available meta data");
            onLastPopulate();
        });
    }
}

$(function(){
    getPackageMetaData(function(data){
        if (!data){
            onLastPopulate();
            return;
        }

        $('#breadcrumb').append($(versionSwitcherHtml)); // Create version switcher select box

        populateVersionSwitcher(data); // Populate version switcher select box (in version-switcher.js)
    });
});