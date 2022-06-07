// Este archivo contiene todos los métodos JS y AJAX para agregar y eliminar
// autores y archivos cuando se está realizando CRUD a un atestado.

var per = 100;
var autorCont = 0;
var modal = document.getElementById('modalAutores');
var checkbox = document.getElementById('AutoresEq');
var hiddenCheck = document.getElementById('hiddenCheck');
var autoresCheck = document.getElementById('AutoresCheck');

function isEmail(e) {
    var filter = /^\s*[\w\-\+_]+(\.[\w\-\+_]+)*\@[\w\-\+_]+\.[\w\-\+_]+(\.[\w\-\+_]+)*\s*$/;
    return String(e).search(filter) != -1;
}

function validPercentage(percentage) {
    return (per - percentage) >= 0 && percentage >= 0;
}

hiddenCheck.onchange = function () {
    document.getElementById('porcentaje').disabled = this.checked;
    document.getElementById('porcentaje_funcionario').disabled = this.checked;
    checkbox.checked = this.checked;
};

// Agregar autores a la tabla de autores.
//$.ajax({
//        type: 'POST',
//        url: `${baseUrl}/getAutores`,
//        async: false,
//        contentType: 'application/json; charset=utf-8',
//        success: function (response) {
//            console.log(response);
//            if (response.length > 0) autoresCheck.checked = true;
//            response.forEach(function (autor) {
//                per -= parseInt(autor.Porcentaje);
//                $("#tablaAutores").append('<tr><td>' + autor.Nombre + '</td><td>' + autor.PrimerApellido + ' ' + autor.SegundoApellido + '</td><td>' + autor.Porcentaje + '%</td> <td><a id="borrar" email="' + autor.Email + '" class="btn btn-danger remove">Borrar</a></td></td></tr>')
//            });
//        }
//    })

$('#subirArchivo').submit(function (e) {
    e.preventDefault();
    var data = new FormData(this);
    if (jQuery('#archivo').val().length != '') {
        $('#archivo').val('');
        $.ajax({
            url: `${baseUrl}/Cargar`,
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (data) {
                var archivo = JSON.parse(data.archivoJson)
                if (data != null) {
                    $("#tablaArchivos").append('<tr><td>' + archivo.Nombre + '</td></tr>')
                    //$("#tablaArchivos").append('<tr><td>' + archivo.Nombre + '</td><td><a>Descargar</a> | <a>Borrar</a></td></tr>')
                }
            }
        })
    }
})

// Agregar funcionarios como autores al libro.
$('#funcionarioAgregar').click(function () {
    var usuario = new Object();
    var funcionario = null;

    usuario.Email = $('#email_funcionario').val();

    $.ajax({
        type: 'POST',
        url: `${baseUrl}/checkFuncionario`,
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(usuario),
        success: function (response) {
            funcionario = JSON.parse(response.usuario)
        }
    });

    if (!funcionario) {
        alert("Funcionario no encontrado");
        return;
    }

    // Validar si el porcentaje ingresado es correcto.
    if (!checkbox.checked && $('#porcentaje_funcionario').val() == "") {
        alert("Debe ingresar un porcentaje");
        return;
    }
    if (!checkbox.checked && !validPercentage(parseInt($('#porcentaje_funcionario').val()))) {
        alert("Se ha ingresado un porcentaje inválido");
        return;
    }

    // Crear el objeto del autor con la información ingresada.
    var autor = new Object();
    autor.Email = usuario.Email;
    autor.esFuncionario = true;
    autor.porcEquitativo = false;
    autor.Porcentaje = $('#porcentaje_funcionario').val();
    autor.numAutor = ++autorCont;
    console.log(JSON.stringify(autor));

    // Si está marcada la opción de porcentaje equitativo.
    if (autor != null && isEmail(autor.Email) && checkbox.checked) {
        autor.porcEquitativo = true;
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarFuncionario`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    // Si no está marcada la opción de porcentaje equitativo.
    else if (autor != null && isEmail(autor.Email)) {
        console.log("wena lolas")
        per = per - parseInt(autor.Porcentaje);
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarFuncionario`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    clearAuthorForm();
});

// Agregar un agente externo como autor.
$('#autorAgregar').click(function () {

    // Validar que el porcentaje ingresado es correcto.
    if (!checkbox.checked && $('#porcentaje').val() == "") {
        alert("Debe ingresar un porcentaje");
        return;
    }
    if (!checkbox.checked && !validPercentage(parseInt($('#porcentaje').val()))) {
        alert("Se ha ingresado un porcentaje inválido");
        return;
    }

    // Crear el objeto del autor con la información ingresada.
    var autor = new Object();
    autor.Nombre = $('#nombre').val();
    autor.PrimerApellido = $('#apellido1').val();
    autor.SegundoApellido = $('#apellido2').val();
    autor.Email = $('#email').val();
    autor.esFuncionario = false;
    autor.porcEquitativo = false;
    autor.Porcentaje = $('#porcentaje').val()
    autor.numAutor = ++autorCont;

    // Si está marcada la opción de porcentaje equitativo.
    if (autor != null && isEmail(autor.Email) && checkbox.checked) {
        autor.porcEquitativo = true;
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarAutor`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    // Si no está marcada la opción de porcentaje equitativo.
    else if (autor != null && isEmail(autor.Email)) {
        per = per - parseInt(autor.Porcentaje);
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarAutor`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    clearAuthorForm();
})

// Quitar texto de todos los campos del formulario de autores.
function clearAuthorForm() {
    $('#nombre').val('');
    $('#apellido1').val('');
    $('#apellido2').val('');
    $('#email').val('');
    $('#porcentaje').val('');
    $('#email_funcionario').val('');
    $('#porcentaje_funcionario').val('');
}

// Borrar autores de la tabla.
$('#tablaAutores').on('click', '.remove', function () {

    var autor = new Object()
    autor.Email = $(this).attr('email');
    var child = $(this).closest('tr').nextAll();
    child.each(function () {
        var id = $(this).attr('id');
        var idx = $(this).children('.row-index').children('p');
        var dig = parseInt(id.substring(1));
        idx.html(`Row ${dig - 1}`);
        $(this).attr('id', `R${dig - 1}`);
    });
    console.log($(this).attr('email'));
    $(this).closest('tr').remove();

    $.ajax({
        type: 'POST',
        url: `${baseUrl}/borrarAutor`,
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(autor),
        success: function (response) {
            console.log("autor eliminado");
        }
    })

});


