#include "imgui.h"
#include "imnodes.h"
#include "backends/imgui_impl_glfw.h"
#include "backends/imgui_impl_opengl3.h"
#include <iostream>
#include "../deps/glfw/include/GLFW/glfw3.h"

int main() {
    if (!glfwInit()) return -1;

    GLFWwindow* window = glfwCreateWindow(800, 600, "ImNodes Demo", nullptr, nullptr);
    glfwMakeContextCurrent(window);

    // Crear contextos
    ImGui::CreateContext();
    ImNodes::CreateContext();

    // Inicializar backends
    ImGui_ImplGlfw_InitForOpenGL(window, true);
    ImGui_ImplOpenGL3_Init("#version 130");

    ImNodes::StyleColorsDark();

    int link_id = 1;

    while (!glfwWindowShouldClose(window)) {
        glfwPollEvents();

        ImGui_ImplOpenGL3_NewFrame();
        ImGui_ImplGlfw_NewFrame();
        ImGui::NewFrame();

        ImNodes::BeginNodeEditor();

        // Nodo 1
        ImNodes::BeginNode(1); // ID unico del nodo
        ImNodes::BeginNodeTitleBar();
        ImGui::Text("Nodo 1"); // Titulo del nodo
        ImNodes::EndNodeTitleBar();

        // Pin de entrada
        ImNodes::BeginInputAttribute(2); 
        ImGui::Text("Input");
        ImNodes::EndInputAttribute();

        // Pin de salida
        ImNodes::BeginOutputAttribute(3);
        ImGui::Text("Output");
        ImNodes::EndOutputAttribute();

        ImNodes::EndNode();

        ImNodes::EndNodeEditor();

        ImGui::Render();
        int display_w, display_h;
        glfwGetFramebufferSize(window, &display_w, &display_h);
        glViewport(0, 0, display_w, display_h);
        glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());

        glfwSwapBuffers(window);
    }

    // Liberar contextos
    ImNodes::DestroyContext();
    ImGui_ImplOpenGL3_Shutdown();
    ImGui_ImplGlfw_Shutdown();
    ImGui::DestroyContext();
    glfwDestroyWindow(window);
    glfwTerminate();
    return 0;
}
