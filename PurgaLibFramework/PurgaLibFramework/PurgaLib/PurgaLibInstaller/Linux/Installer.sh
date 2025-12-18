#!/bin/bash

echo "=== PurgaLib DLL Installer ==="

read -p "Enter the path to the SCP:SL server folder: " SCP_PATH

if [ ! -d "$SCP_PATH" ]; then
  echo "Invalid path"
  exit 1
fi

PLUGIN_PATH="$SCP_PATH/LabAPI/Plugins/global"
PURGA_DLL="$(dirname "$0")/PurgaLibFramework.dll"

if [ ! -f "$PURGA_DLL" ]; then
  echo "PurgaLibFramework.dll not found in the same folder as this script"
  exit 1
fi

mkdir -p "$PLUGIN_PATH"
cp -f "$PURGA_DLL" "$PLUGIN_PATH"

echo "DLL copied to $PLUGIN_PATH"
echo "Installation completed!"
