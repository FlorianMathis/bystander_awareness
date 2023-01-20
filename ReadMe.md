# BANS: Evaluation of Bystander Awareness Notification Systems for Productivity in Virtual Reality

![We explore the use of BANS to support VR users' privacy in public spaces.](/Figures/FinalTeaser.png?raw=true "We explore the use of BANS to support VR users' privacy in public spaces.")

*Figure 1: We explore the use of BANS to support VR users' privacy in public spaces. We compare the usability of different BANS, i.e., from least to most displayed information, and their impact on VR users' productivity and sense of presence. Figures (a) to (f) illustrate the BANS, described in more detail in the paper.*

ABSTRACT VR Head-Mounted Displays (HMDs) provide unlimited and personalized virtual workspaces and will enable working anytime and anywhere. However, if HMDs are to become ubiquitous, VR users are at risk of being observed, which can threaten their privacy. We examine six <ins>B</ins>ystander <ins>A</ins>wareness <ins>N</ins>otification <ins>S</ins>ystems (BANS) to enhance VR users' bystander awareness whilst immersed in VR. In a user study (N=28), we explore how future HMDs equipped with BANS might enable users to maintain their privacy while contributing towards enjoyable and productive travels. Results indicate that BANS increase VR users' bystander awareness without affecting presence and productivity.Users prefer BANS that extract and present the most details of reality to facilitate their bystander awareness.We conclude by synthesizing four recommendations, such as providing VR users with control over BANS and considering how VR users can best transition between realities, to inform the design of privacy-preserving HMDs. 

Links: [Paper (probably not available yet)](https://doi.org/10.1145/3491102.3501821)


## Table of Contents
1. [Overview of the BANS](#overview-of-the-bans)
2. [Install the APK on the Oculus Quest](#install-the-apk-on-the-oculus-quest)
3. [Third-Party Content](#third-party-content)


## Overview of the BANS
We implemented seven BANS (including a baseline) in VR, all of which provide different levels of reality awareness and are situated at different levels on the bystander awareness continuum. Figure 1 provides an overview of all studied BANS and Figure 2 shows the same BANS in a virtual workspace environment. Auditory feedback (i.e., a *beep tone*) was included in all BANS as a modality to direct users' attention to the visual notifications, as suggested by [[1]](#references). All BANS were triggered automatically after a five second VR experience, similar to previous works [[2,3]](#references). Each notification stayed visible for up to ten seconds before disappearing automatically. 


The BANS were designed to allow for intentionally dismissing them by continuing with the VR productivity task (i.e., typing on the physical keyboard). Introducing an intentional dismissal feature for all BANS provides VR users with full control of the BANS and is a common approach when designing and implementing notification systems [[4]](#references) and has previously been identified as future work when designing notification systems for VR [[5]](#references). A buffer of three seconds (determined through pilot tests) was used to lock manually dismissing the notification and to allow VR users to perceive the notification and not accidentally dismiss it. The following six BANS were implemented to evaluate their usability and impact on the VR users' productivity and perceived sense of presence. 


![Snippets of the six BANS:](/Figures/SnippetsHorizontal.png?raw=true "Workflows and Environments of Snippets of the six BANS:")

*Figure 2: Snippets of the six BANS: **A)** Text UI, **B)** Avatar, **C)** 2D-Radar, **D)** Attention Marker, **E)** 3D-Scan, **F)** Passthrough.*

### Requirements
* Oculus App on the computer
* USB cable to connect the VR device to the computer
* Oculus Quest 2 headset

### Enable Developer Mode
1. Turn on the Oculus VR headset
2. Open the Oculus app (on your phone) and go to 'Settings'
3. Connect to your device and go to 'More Settings'
4. Enable the 'Developer Mode'

### How to Install an APK via ADB
1. Download and install ADB (Android Debug Bridge), for more info and instruction: https://developer.android.com/studio/releases/platform-tools and https://developer.android.com/studio/command-line/adb
2. Download the APK file from the 'Build' folder
3. Open the CMD/Terminal and navigate to the <platform-tools> folder
4. Connect your VR headset with the USB cable and allow permission in Oculus, when asked
5. Check that the device is connected/listed with the command `adb devices`
6. Install the .apk file with `adb install <apk-path>`
  
### How to Install an APK via SideQuest
1. Download and install the SideQuest software from the [offical website](https://sidequestvr.com/setup-howto)
2. Download one of the two APK files from the 'Build' folder
3. Connect your VR headset with the USB cable and allow permission in Oculus, when asked
4. Start the SideQuest software and select 'Install APK file from folder on computer' represented as an icon in the top menu bar
5. Select the APK you want to install and SideQuest should automatically install it



## Third-Party Content
To implement the Bans and the virtual environments, we used different software and assets provided by third parties. Unfortunately, not all third-party content is freely available and thus, cannot be offered with the source files. Nevertheless, the basic functionality of the Bans is not limited by some missing software and assets. Moreover, we provide an APK of the complete environment including the paid content and made it easy to add the content back into the Unity project after buying it.
  
### Third-Party Content (freely available) - Included in This Repository
  
* Software: Oculus XR Plugin (https://docs.unity3d.com/Packages/com.unity.xr.oculus@1.4/manual/index.html)
* Software: Oculus Unity Integration (https://developer.oculus.com/downloads/package/unity-integration)
* Software: Text Mesh Pro (https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.2/manual/index.html)
* Asset: Oculus quest 2 (https://assetstore.unity.com/packages/3d/props/vr-headset-vol-1-161024)
* Asset: Modern Furniture Pack (https://skfb.ly/ooFoL)
* Asset: Radar (https://unitycodemonkey.com/downloadpage.php?yid=J0gmrgpx6gk)

### Third-Party Content (paid) - NOT Included in This Repository
* Asset: Train Interior VR (https://assetstore.unity.com/packages/3d/vehicles/train-interior-vr-144344)
* Asset: Summer Beach Cartoon Pack - VR/Mobile (https://assetstore.unity.com/packages/3d/environments/summer-beach-cartoon-pack-vr-mobile-17444)
* Asset: 3D Office Furniture (https://assetstore.unity.com/packages/3d/props/interior/3d-office-furniture-34262)



  
## References
[1] S. Ghosh, L. Winston, N. Panchal, P. KimuraThollander, J. Hotnog, D. Cheong, G. Reyes, and G. D. Abowd, “Notifivr: Exploring interruptions and notifications in virtual reality,” IEEE Transactions on Visualization and Computer Graphics, vol. 24, no. 4, p. 1447–1456, apr 2018. [Online]. Available: https://doi.org/10.1109/TVCG.2018.2793698  

[2] J. O’Hagan and J. R. Williamson, “Reality aware vr headsets,” in Proceedings of the 9TH ACM International Symposium on Pervasive Displays, 2020, pp. 9–17.

[3] M. Gottsacker, N. Norouzi, K. Kim, G. Bruder, and G. Welch, “Diegetic representations for seamless cross-reality interruptions,” in 2021 IEEE International Symposium on Mixed and Augmented Reality (ISMAR), 2021, pp. 310–319.

[4] D. Weber, A. Voit, J. Auda, S. Schneegass, and N. Henze, “Snooze! investigating the user-defined deferral of mobile notifications,” in Proceedings of the 20th International Conference on Human-Computer Interaction with Mobile Devices and Services, 2018, pp. 1–13.
  
[5] R. Rzayev, S. Mayer, C. Krauter, and N. Henze, “Notification in vr: The effect of notification placement, task and environment,” in Proceedings of the Annual Symposium on Computer-Human Interaction in Play, ser. CHI PLAY ’19. New York, NY, USA: Association for Computing Machinery, 2019, p. 199–211. [Online]. Available: https://doi.org/10.1145/3311350.3347190
