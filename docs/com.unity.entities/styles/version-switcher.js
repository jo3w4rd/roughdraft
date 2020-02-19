let versionsAdded = 0;
let versionsToAdd = [];

function populateVersionSwitcher(metadata){
    let versionsInfo = metadata.versions;
    let versionsToPopulate = {};
    let numberToPopulate = 0;
    let populatedCounter = 0;

    for (var version in versionsInfo){
        let versionSplit = version.split('.');
        let versionTrimmed = versionSplit[0] + '.' + versionSplit[1];
        let currentVersionSplit = thisPackageMetaData.version.split('.');
        let currentVersionTrimmed = currentVersionSplit[0] + '.' + currentVersionSplit[1];

        if (!versionsToPopulate[versionTrimmed] && versionTrimmed != currentVersionTrimmed){
            versionsToPopulate[versionTrimmed] = {versionTrimmed: versionTrimmed};
            numberToPopulate++;
        }
    }

    var versionsToPopulateLength = 0;

    for (var versionTrimmed in versionsToPopulate){
        versionsToPopulateLength++;
        (function(version){
            let gotoUrl = getPageUrlFromVersion(version);

        $.ajax( gotoUrl )
            .done(function() {
                addToList(version, gotoUrl);
                versionsAdded++;
                if (++populatedCounter >= numberToPopulate){
                    onLastPopulate();
                }
            })
            .fail(function() {
                let indexUrl = getIndexURL(version);

                $.ajax( indexUrl )
                    .done(function() {
                        addToList(version, indexUrl);
                        versionsAdded++;
                        if (++populatedCounter >= numberToPopulate){
                            onLastPopulate();
                        }
                    })
                    .fail(function() {
                        if (++populatedCounter >= numberToPopulate){
                            onLastPopulate();
                        }
                    });  
            });        
        })(versionTrimmed);
    }

    if (versionsToPopulateLength <= 0){
        onLastPopulate();
    }
    
}

function onLastPopulate(){
    if (versionsAdded <= 0){
        $('#version-switcher-select').remove();
        $('#breadcrumb').append($('<p style="margin: 10px 0;"><b>' + thisPackageMetaData.displayTitle + '</b></p>'));
    }
    else {
        versionsToAdd.sort((a,b) => (Number(a.version) < Number(b.version)) ? 1 : ((Number(b.version) < Number(a.version)) ? -1 : 0));
        console.log(versionsToAdd);

        for (var i = 0; i < versionsToAdd.length; i++){
            createVersionOption(versionsToAdd[i].version, versionsToAdd[i].gotoUrl);
        }

        onVersionSwitcherClick();
    }
}

function addToList(version, gotoUrl){
    versionsToAdd.push({version:version, gotoUrl:gotoUrl});
}

function createVersionOption(version, gotoUrl){
    let item = $(`<a style="color:#000;" href="` + gotoUrl + `"><li class="component-select__option">
                    ` +version + `
                </li></a>`);
    $('#version-switcher-ul').append(item);
}

function getPageUrlFromVersion(versionTrimmed){
    return location.protocol + "//" + location.host + "/" + location.pathname.split('/')[1] + "/" + packageName + "@" + versionTrimmed + "/" + location.pathname.split('/').splice(3).join('/');
}

function getIndexURL(versionTrimmed){
    return location.protocol + "//" + location.host + "/" + location.pathname.split('/')[1] + "/" + packageName + "@" + versionTrimmed + "/index.html";
}

function onVersionSwitcherClick(){
    $('#component-select-current-display').click(function(){
        $('#component-select-current-display').toggleClass('component-select__current--is-active');
    });
}

$(document).click(function(e){
    console.log(!(e.target.id == 'component-select-current-display'));
    if (!(e.target.id == 'component-select-current-display'))
        $('#component-select-current-display').removeClass('component-select__current--is-active');
});