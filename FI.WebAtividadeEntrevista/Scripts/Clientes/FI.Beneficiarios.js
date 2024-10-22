function IncluirBeneficiario() {

    var id = document.getElementById('IdBeneficiario').value || 0
    var cpf = document.getElementById('CPFBeneficiario').value || ''
    var nome = document.getElementById('NomeBeneficiario').value || ''

    if (!cpf) {
        ModalDialog('Beneficiarios', 'O campo CPF do beneficiario e obrigatorio')
        return
    }

    if (!nome) {
        ModalDialog('Beneficiarios', 'O campo nome do beneficiario e obrigatorio')
        return
    }

    var beneficiario = {
        "Id": id,
        "Nome": nome,
        "CPF": cpf
    }

    const beneficiarioPesquisar = beneficiarios.find(item => item.CPF == cpf) || null;

    if (beneficiarioPesquisar) {
        ModalDialog("Beneficiario", "Ja existe um beneficiario com o CPF informado para este cliente")
        return
    }

    ValidarBeneficiario(beneficiario)
}

function ValidarBeneficiario(beneficiario) {

    $.ajax({
        url: urlValidar,
        method: "POST",
        data: beneficiario,
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {

                CarregarBeneficiario(beneficiario.Id, beneficiario.CPF, beneficiario.Nome)

                if (beneficiario.Id > 0) {

                    const index = beneficiarios.findIndex(item => item.Id == beneficiario.Id);

                    beneficiarios.splice(index, 1);

                    beneficiarios.push(beneficiario)

                    var table = document.getElementById("beneficiarioTable");

                    table.innerHTML = ''

                    CarregarBeneficiarios(beneficiarios)
                }
                else {
                    beneficiarios.push(beneficiario)
                }

                document.getElementById('IdBeneficiario').value = 0
                document.getElementById('CPFBeneficiario').value = ''
                document.getElementById('NomeBeneficiario').value = ''
            }
    });
}

function CarregarBeneficiarios(beneficiariosCarregar) {

    for (var i = 0; i < beneficiariosCarregar.length; i++) {

        if (beneficiariosCarregar[i].Deletar)
            continue;

        CarregarBeneficiario(beneficiariosCarregar[i].Id, beneficiariosCarregar[i].CPF, beneficiariosCarregar[i].Nome)
    }
}

function CarregarBeneficiario(id, cpf, nome) {

    var table = document.getElementById("beneficiarioTable");

    var row = table.insertRow();

    var cell0 = row.insertCell(0);
    var cell1 = row.insertCell(1);
    var cell2 = row.insertCell(2);
    var cell3 = row.insertCell(3);

    cell0.innerHTML = id
    cell1.innerHTML = cpf;
    cell2.innerHTML = nome;
    cell3.innerHTML = "<button onclick=\"CarregarDados(this)\" type=\"button\" class=\"btn btn-sm btn-primary\">Alterar</button> <button onclick=\"DeletarBeneficiario(this)\" type=\"button\" class=\"btn btn-sm btn-primary\">Excluir</button> ";
}

function CarregarDados(button) {

    var row = button.parentNode.parentNode;

    var id = row.cells[0].innerHTML || '';
    var cpf = row.cells[1].innerHTML || '';
    var nome = row.cells[2].innerHTML || '';

    document.getElementById('IdBeneficiario').value = id
    document.getElementById('CPFBeneficiario').value = cpf
    document.getElementById('NomeBeneficiario').value = nome

    if (id < 1) {

        row.parentNode.removeChild(row);

        const index = beneficiarios.findIndex(item => item.CPF == cpf);
        beneficiarios.splice(index, 1);
    }

}

function DeletarBeneficiario(button) {

    var row = button.parentNode.parentNode;

    var id = row.cells[0].innerHTML || '';

    const index = beneficiarios.findIndex(item => item.Id == id);
    const beneficiario = beneficiarios.find(item => item.Id == id);

    beneficiario.Deletar = true

    beneficiarios.splice(index, 1);

    beneficiarios.push(beneficiario)

    var table = document.getElementById("beneficiarioTable");

    table.innerHTML = ''

    CarregarBeneficiarios(beneficiarios)
}

$(document).ready(function () {
    $('#CPFBeneficiario').mask('000.000.000-00');
});

function AbrirModalBenediciario(beneficiarios) {
    $('#modalBeneficiario').modal('show');
}