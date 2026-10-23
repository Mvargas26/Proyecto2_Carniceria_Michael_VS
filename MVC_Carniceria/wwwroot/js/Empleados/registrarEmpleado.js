document.addEventListener("DOMContentLoaded", function () {
    const selectArea = document.getElementById("selectArea");
    const selectPuesto = document.getElementById("selectPuesto");

    function filtrarPuestos() {
        const areaSeleccionada = selectArea.value;
        const puestosValidos = mapeoAreaPuesto[areaSeleccionada] || [];

        for (const opcion of selectPuesto.options) {
            opcion.hidden = !puestosValidos.includes(opcion.value);
        }

         // Si el puesto actualmente seleccionado ya no pertenece al área, lo limpiamos
        if (!puestosValidos.includes(selectPuesto.value)) {
            selectPuesto.value = "";
        }

    }//fn funcion

    selectArea.addEventListener("change", filtrarPuestos);

    // Filtra al cargar la página, por si el área ya viene seleccionada (ej. en Editar)
    filtrarPuestos();
});