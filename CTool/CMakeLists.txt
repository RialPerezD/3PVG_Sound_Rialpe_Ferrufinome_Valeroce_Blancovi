cmake_minimum_required(VERSION 3.20)

project(GameGustavo LANGUAGES C CXX)

# -------------------------------
#   Configuración del lenguaje
# -------------------------------
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# -------------------------------
#   Rutas de cabeceras
# -------------------------------
include_directories(
    ${PROJECT_SOURCE_DIR}/include
    ${PROJECT_SOURCE_DIR}/deps/imgui       # ImGui
    ${PROJECT_SOURCE_DIR}/deps/imgui/backends
    ${PROJECT_SOURCE_DIR}/deps/imnodes     # ImNodes
    ${PROJECT_SOURCE_DIR}/deps/glfw/include  # GLFW
)

# -------------------------------
#   Archivos fuente
# -------------------------------
file(GLOB SRC_FILES_TOOL 
    ${PROJECT_SOURCE_DIR}/src/mainTool.cpp
    ${PROJECT_SOURCE_DIR}/src/sound.cpp
)

# -------------------------------
#   Compilar ImGui (librería interna)
# -------------------------------
set(IMGUI_DIR ${PROJECT_SOURCE_DIR}/deps/imgui)
file(GLOB IMGUI_SOURCES
    ${IMGUI_DIR}/*.cpp
)

add_library(imgui_internal STATIC ${IMGUI_SOURCES})
set_target_properties(imgui_internal PROPERTIES EXCLUDE_FROM_ALL TRUE)
target_include_directories(imgui_internal PUBLIC ${IMGUI_DIR})

# -------------------------------
#   Compilar backends de ImGui
# -------------------------------
add_library(imgui_backends STATIC
    ${IMGUI_DIR}/backends/imgui_impl_glfw.cpp
    ${IMGUI_DIR}/backends/imgui_impl_opengl3.cpp
)
target_include_directories(imgui_backends PUBLIC
    ${IMGUI_DIR}
    ${IMGUI_DIR}/backends
    ${PROJECT_SOURCE_DIR}/deps/glfw/include
)
# Link directo a glfw3.lib
target_link_libraries(imgui_backends PUBLIC imgui_internal
    ${PROJECT_SOURCE_DIR}/deps/glfw/lib/glfw3.lib
)

# -------------------------------
#   Compilar ImNodes (librería interna)
# -------------------------------
set(IMNODES_DIR ${PROJECT_SOURCE_DIR}/deps/imnodes)
add_library(imnodes_internal STATIC ${IMNODES_DIR}/imnodes.cpp)
set_target_properties(imnodes_internal PROPERTIES EXCLUDE_FROM_ALL TRUE)
target_include_directories(imnodes_internal PUBLIC ${IMNODES_DIR})
target_link_libraries(imnodes_internal PUBLIC imgui_internal)

# -------------------------------
#   Ejecutable final
# -------------------------------
add_executable(Tool ${SRC_FILES_TOOL})

# -------------------------------
#   Forzar aplicación de consola
# -------------------------------
set_target_properties(Tool PROPERTIES WIN32_EXECUTABLE OFF)

# -------------------------------
#   Librerías externas
# -------------------------------
target_link_libraries(Tool
    ${PROJECT_SOURCE_DIR}/deps/OpenAL/libs/Win64/OpenAL32.lib
    opengl32.lib
    user32.lib
    gdi32.lib
    shell32.lib
    Ws2_32.lib
    ${PROJECT_SOURCE_DIR}/deps/glfw/lib/glfw3.lib  # link correcto
    imgui_internal
    imgui_backends
    imnodes_internal
)

if(MSVC)
    set_target_properties(Tool PROPERTIES LINK_FLAGS "/SAFESEH:NO")
endif()

# -------------------------------
#   Carpeta donde se generará el exe
# -------------------------------
set_target_properties(Tool PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY ${PROJECT_SOURCE_DIR}/build
)

# -------------------------------
#   Copiar DLL junto al ejecutable (OpenAL y GLFW si usas DLL)
# -------------------------------
add_custom_command(TARGET Tool POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "${PROJECT_SOURCE_DIR}/OpenAl32.dll"
        "$<TARGET_FILE_DIR:Tool>"
)

# -------------------------------
#   Carpeta de solución para organizar
# -------------------------------
set_target_properties(Tool PROPERTIES FOLDER "Tool")
