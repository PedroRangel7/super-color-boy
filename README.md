# Unity+Git+Remote Template
This repository is an Unity + Git project template. The complete setup also includes integration with GitHub, Sourcetree and P4Merge, but any other GUI, remote or merge tool can be used instead.

Just **use this repository as a template** and **follow the steps** described below.
</br>
</br>
## Features
* **Ignores Windows generated files** (cache and temporary files) accordingly to GitHub's official .gitignore Windows template ([Link](https://github.com/github/gitignore/blob/master/Global/Windows.gitignore)).\
&emsp;&emsp;_.gitignore in root folder._
* **Ignores backup files** accordingly to GitHub's official .gitignore backup files template ([Link](https://github.com/github/gitignore/blob/master/Global/Backup.gitignore)).\
&emsp;&emsp;_.gitignore in root folder_
* **Ignores Unity related files** accordingly to GitHub's official .gitignore Unity template ([Link](https://github.com/github/gitignore/blob/master/Unity.gitignore)).\
&emsp;&emsp;_.gitignore in Unity's project folder._
* **Supports Git-LFS** for common 3D models, images, audio, videos, archives and fonts file formats.\
&emsp;&emsp;_.gitattributes in root folder._
* **Supports Unity SmartMerge** (aka Unity YAML Merge) to smartly solve conflicts and merge Unity scenes and prefabs.
* **Supports external Git GUI** (Sourcetree used).
* **Supports external Diff & Merge tools** (P4Merge used).
* **Supports a remote repository** (GitHub used).

___
## Step-By-Step Guide
This step-by-step guide will teach you how to setup:
* **Git + Git-LFS** (Version Control System)
* **GitHub** (Remote Repository)
* **Sourcetree** (Git GUI)
* **P4Merge** (Diff & Merge tool)
* **Unity** (Game Engine)
* **Unity SmartMerge** (Unity tool to enable scene and prefab merging)

While **Git**, **Unity** and **Unity SmartMerge** are necessary, the other tools (**GitHub**, **Sourcetree** and **P4Merge**) can be replaced for other remotes, GUIs and mergetools of your own preference.
### **These steps need to be done only \*once per machine\*.**
</br>

### 1 - Installation
The following steps are only necessary if you don't already have Unity, Git, GitHub, a Git GUI and a Diff & Merge tool installed/configured.

1. Install [Unity](https://store.unity.com/download).

2. Install [Git](https://git-scm.com/downloads). During instalation, be sure to select Git LFS when asked:\
![Git LFS Setup](/Tutorial/Image1.png)

3. Create a [GitHub](https://github.com/) account.

4. Launch Git Bash and run the following commands:
```
git config --global user.name "<< Your GitHub username >>"
git config --global user.email "<< Your GitHub email >>"
```

5. Install the Git GUI ([Sourcetree](https://www.sourcetreeapp.com/)). You will have to create a BitBucket account.\
During installation, be sure to select/deselect accordingly to the image below:\
![Sourcetree Setup](/Tutorial/Image4.png)

6. Install the external Diff & Merge tool ([P4Merge](https://www.perforce.com/downloads/visual-merge-tool)). This is a software specialized in showing differences between files and helping the user solve merge conflicts.\
During installation, be sure to deselect all other apps and install only the Diff and Merge tool P4Merge:\
![P4Merge Setup](/Tutorial/Image2.png)
&nbsp;</br>
&nbsp;</br>
### 2 - Sourcetree + P4Merge + Unity SmartMerge setup
The following steps are only necessary if you don't already have a Git GUI configured.

1. Launch Sourcetree and login to your remote (your GitHub account).

2. Go to `Tools > Options > Diff`.

3. Select "P4Merge" in the _External Diff Tool_ dropdown.

4. Select "Custom" in the _Merge Tool_ dropdown.\
Type the path to UnityYAMLMerge in the _Merge Command_ text field (it's usually located at `C:\Program Files\Unity\Hub\Editor\ <<Unity Version>> \Editor\Data\Tools\UnityYAMLMerge.exe`).\
Type &nbsp;`merge -p $BASE $REMOTE $LOCAL $MERGED`&nbsp; in the _Arguments_ field for the _Merge Tool_.\
![Sourcetree Diff&Merge Setup](/Tutorial/Image3.png)

5. Launch Git Bash and run the following command (it will keep Git from creating lots of backup "`.orig`" files that can quickly clutter up your repository folder when merge conflicts occur):
```
git config --global mergetool.keepBackup false
```

6. _(Ignore this step if you're using P4Merge as a mergetool)_\
Open the `mergespecfile.txt` file (located in the same folder as the `UnityYAMLMerge.exe`, usually at `C:\Program Files\Unity\Hub\Editor\ <<Unity Version>> \Editor\Data\Tools\mergespecfile.txt`). This file describes which Diff & Merge tool will be used when Unity SmartMerge can't solve merge conflicts without human intervention and which Diff & Merge tool will be used for non-Unity files.\
There are some preconfigured Diff & Merge tools listed in the file. If the tool you want to use is already listed, you don't have to do anything (P4Merge, for example, is already listed). Otherwise, replace the following lines:
```
unity use "%programs%\YouFallbackMergeToolForScenesHere.exe" "%l" "%r" "%b" "%d"
prefab use "%programs%\YouFallbackMergeToolForPrefabsHere.exe" "%l" "%r" "%b" "%d"
```

with:

```
unity use "<< path to yourMergeTool.exe >>" "%l" "%r" "%b" "%d"
prefab use "<< path to yourMergeTool.exe >>" "%l" "%r" "%b" "%d"
```
&nbsp;</br>
### 3 - Unity project setup
After you finished the previous steps, you just have to **use this repository as a template for each Unity project you want to start**. It's already configured with everything you need (Git LFS, .gitignore files, .gitattributes file, Unity Editor setup and a folder for your project).

1. Use this repository as a template (click on the green "Use this template" button on [this repository's page](https://github.com/ThiagoRangel7/unity-git-template)).\
!["Use this template" button](/Tutorial/Image5.png)\
This will copy the folder structure and files of this repository to a new repository of your own.

2. Open your newly copied repository's page on GitHub and click on the green "Clone or download" button. Copy the link on the popup window.\
!["Clone or download" button](/Tutorial/Image6.png)

3. Open Sourcetree, click on the "Clone" button, paste the link, choose the destination folder and confirm. This will download the repository to your computer and automatically link it to your GitHub account.\
![Sourcetree clone](/Tutorial/Image7.png)

4. Open the chosen destination folder (the cloned repository folder on your computer) and delete `README.md` file and `Tutorial` folder.

5. Rename &nbsp;`Unity Project Folder`&nbsp; to the project name you want.

6. Open Unity Hub (aka Unity Launcher), go to Projects tab and click the "ADD" button. Select the Unity Project Folder you renamed on step 5.

Make a game.
