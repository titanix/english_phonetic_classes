#!/bin/sh

if [ $# -eq 1 ] && [ $1 = "build" ]
then
    echo "Building executables"
    mkdir bin
    csc code/cmu.cs /out:bin/cmu.exe
    csc code/ngsl_ipa.cs /out:bin/ngsl_ipa.exe
    csc code/classes.cs /out:bin/classes.exe
fi

echo "Step 1: Converting CMU dictionary to IPA script"
echo "IN: cmudict-0.7b.txt, OUT: cmu_ipa.txt\n"
mono bin/cmu.exe cmudict-0.7b.txt cmu_ipa.txt

echo "Step 2: Extracting NGSL vocabulary"
echo "IN: ngsl.txt, cmu_ipa.txt, OUT: ngsl_ipa.txt\n"
mono bin/ngsl_ipa.exe ngsl.txt cmu_ipa.txt ngsl_ipa.txt

echo "Step 3: Converting NGSL/IPA to classes."
echo "IN: ngsl_ipa.txt, classes.txt, OUT: final_output.txt"
mono bin/classes.exe ngsl_ipa.txt classes.txt final_output.txt