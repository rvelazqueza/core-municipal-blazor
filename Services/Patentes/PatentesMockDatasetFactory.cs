using BlazorApp.Models;

namespace BlazorApp.Services;

internal static class PatentesMockDatasetFactory
{
    public static PatentesModuleSnapshotDto BuildSnapshot(List<LicenciaComercialDto> licencias)
    {
        var solicitudes = licencias.Select(x => CloneSolicitud(x.Solicitud)).OrderByDescending(x => x.FechaSolicitud).ToList();
        var movimientos = licencias.SelectMany(x => x.Movimientos).OrderByDescending(x => x.FechaHora).ToList();
        var historial = BuildHistory(licencias, movimientos);

        return new PatentesModuleSnapshotDto
        {
            LicenciasComerciales = licencias.Select(CloneLicencia).OrderByDescending(x => x.FechaSolicitud).ToList(),
            Solicitudes = solicitudes,
            Contribuyentes = BuildApplicants(),
            Locales = BuildLocations(),
            ActividadesEconomicas = BuildActivities(),
            ValidacionesUsoSuelo = BuildLandUse(licencias),
            Requisitos = licencias.SelectMany(x => x.Requisitos).Select(CloneRequirement).Take(20).ToList(),
            HaciendaSeguridadSocial = BuildCompliance(licencias),
            Morosidad = BuildMorosity(licencias),
            Tasaciones = BuildAssessments(licencias),
            Resoluciones = BuildResolutions(licencias),
            CuentasPorCobrar = BuildReceivables(licencias),
            Pagos = BuildPayments(licencias),
            CuentaTributaria = BuildTaxAccount(licencias),
            Declaraciones = BuildDeclarations(licencias),
            Exoneraciones = BuildExemptions(licencias),
            Emisiones = BuildEmissions(),
            Movimientos = movimientos.Select(CloneMovement).Take(20).ToList(),
            LicenciasTemporales = BuildTemporaryLicenses(),
            LicenciasLicores = BuildLiquorLicenses(licencias),
            CapitalAccionario = BuildShareCapital(),
            Inspecciones = BuildInspections(licencias),
            Infracciones = BuildInfractions(),
            ProcesosAdministrativos = BuildAdministrativeProcesses(),
            Denuncias = BuildComplaints(licencias),
            Notificaciones = BuildNotifications(licencias),
            Historial = historial,
            Auditoria = BuildAuditEvents(movimientos),
            ControlCalidad = BuildQualityCases(),
            Reportes = BuildReports(),
            Catalogos = BuildCatalogs(),
            Integraciones = BuildIntegrations()
        };
    }

    private static List<PatApplicantDto> BuildApplicants() => new()
    {
        new() { Id = 1, Identificacion = "1-1234-5678", Nombre = "Ana Solano Perez", TipoPersona = "Física", Correo = "ana.solano@demo.go.cr", Telefono = "2222-1101", DireccionFiscal = "Carmen, avenida central", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Completa" },
        new() { Id = 2, Identificacion = "3-101-998877", Nombre = "Tecnologias Urbanas CR S.A.", TipoPersona = "Jurídica", Correo = "contacto@tecnocentro.cr", Telefono = "2288-4455", DireccionFiscal = "San Francisco, parque empresarial", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Validada" },
        new() { Id = 3, Identificacion = "1555666777", Nombre = "Luis Mora Campos", TipoPersona = "Física", Correo = "lmora@demo.go.cr", Telefono = "2280-2233", DireccionFiscal = "Hospital, calle 8", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Completa" },
        new() { Id = 4, Identificacion = "2-3456-7890", Nombre = "Distribuidora El Roble S.R.L.", TipoPersona = "Jurídica", Correo = "gerencia@elroble.cr", Telefono = "2234-5601", DireccionFiscal = "Merced, costado sur del parque", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Observada" },
        new() { Id = 5, Identificacion = "1-9876-5432", Nombre = "María Fernanda Quesada", TipoPersona = "Física", Correo = "mfquesada@demo.go.cr", Telefono = "2266-1122", DireccionFiscal = "Pavas, residencial oeste", MedioNotificacion = "SMS", Estado = "Activo", CalidadDatosRuc = "Completa" },
        new() { Id = 6, Identificacion = "3-102-456789", Nombre = "Eventos Metropolitanos S.A.", TipoPersona = "Jurídica", Correo = "tramites@eventosmetro.cr", Telefono = "2255-9977", DireccionFiscal = "Mata Redonda, oficentro ejecutivo", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Completa" },
        new() { Id = 7, Identificacion = "1-4455-6677", Nombre = "Jorge Calvo Rojas", TipoPersona = "Física", Correo = "jcalvo@demo.go.cr", Telefono = "2229-0088", DireccionFiscal = "Hatillo, avenida 12", MedioNotificacion = "Domicilio fiscal", Estado = "Pendiente actualización", CalidadDatosRuc = "Parcial" },
        new() { Id = 8, Identificacion = "3-101-223344", Nombre = "Logística del Valle S.A.", TipoPersona = "Jurídica", Correo = "operaciones@logvalle.cr", Telefono = "2277-6600", DireccionFiscal = "Uruca, bodegas centrales", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Validada" },
        new() { Id = 9, Identificacion = "1-2233-4455", Nombre = "Karen Brenes Alpízar", TipoPersona = "Física", Correo = "kbrenes@demo.go.cr", Telefono = "2244-7711", DireccionFiscal = "Zapote, frente a plaza del distrito", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Completa" },
        new() { Id = 10, Identificacion = "3-102-998877", Nombre = "Servicios Gastronómicos del Centro S.A.", TipoPersona = "Jurídica", Correo = "legal@sgcentro.cr", Telefono = "2200-9080", DireccionFiscal = "Catedral, avenida 4", MedioNotificacion = "Correo", Estado = "Activo", CalidadDatosRuc = "Validada" }
    };

    private static List<PatBusinessLocationDto> BuildLocations() => new()
    {
        new() { Id = 1, IdPredial = "SJ-889900", NumeroFinca = "1-098765", Direccion = "Centro corporativo norte, local 2", Distrito = "San Francisco", Propietario = "Tecnologias Urbanas CR S.A.", CondicionOcupacion = "Arrendatario", AreaLocal = 95, CuentaServiciosMunicipales = "CSM-24001", NombreLocal = "TecnoCentro", EstadoGis = "Validado", EstadoCuentaTributaria = "Al día" },
        new() { Id = 2, IdPredial = "SJ-002145", NumeroFinca = "1-034566", Direccion = "Boulevard central, local esquinero", Distrito = "Carmen", Propietario = "Inversiones del Centro S.A.", CondicionOcupacion = "Arrendatario", AreaLocal = 120, CuentaServiciosMunicipales = "CSM-24002", NombreLocal = "Cafe Central", EstadoGis = "Pendiente capa", EstadoCuentaTributaria = "Pendiente validación" },
        new() { Id = 3, IdPredial = "SJ-550011", NumeroFinca = "1-088877", Direccion = "Calle 8, frente al parque del distrito", Distrito = "Hospital", Propietario = "Luis Mora Campos", CondicionOcupacion = "Propietario", AreaLocal = 180, CuentaServiciosMunicipales = "CSM-24003", NombreLocal = "Ferretería Hospital", EstadoGis = "Observado", EstadoCuentaTributaria = "Cobro pendiente" },
        new() { Id = 4, IdPredial = "SJ-778811", NumeroFinca = "1-077711", Direccion = "Parque empresarial este, bodega 5", Distrito = "Pavas", Propietario = "Tecnologias Urbanas CR S.A.", CondicionOcupacion = "Propietario", AreaLocal = 250, CuentaServiciosMunicipales = "CSM-24004", NombreLocal = "Bodega Urbana", EstadoGis = "Validado", EstadoCuentaTributaria = "Al día" },
        new() { Id = 5, IdPredial = "SJ-009901", NumeroFinca = "1-029911", Direccion = "Frente a la estación, local 4", Distrito = "Merced", Propietario = "Distribuidora El Roble S.R.L.", CondicionOcupacion = "Propietario", AreaLocal = 140, CuentaServiciosMunicipales = "CSM-24005", NombreLocal = "Licorera El Roble", EstadoGis = "Validado", EstadoCuentaTributaria = "Arreglo de pago" },
        new() { Id = 6, IdPredial = "SJ-450032", NumeroFinca = "1-067890", Direccion = "Costado norte del gimnasio municipal", Distrito = "Hatillo", Propietario = "Municipalidad", CondicionOcupacion = "Ocupante autorizado", AreaLocal = 60, CuentaServiciosMunicipales = "CSM-24006", NombreLocal = "Feria Gastronómica", EstadoGis = "Pendiente", EstadoCuentaTributaria = "No aplica" },
        new() { Id = 7, IdPredial = "SIN-LOCAL-01", NumeroFinca = "No aplica", Direccion = "Actividad ambulante por rutas autorizadas", Distrito = "Catedral", Propietario = "No aplica", CondicionOcupacion = "No aplica", AreaLocal = 0, CuentaServiciosMunicipales = "No aplica", NombreLocal = "Servicio Técnico Móvil", EstadoGis = "Referencial", EstadoCuentaTributaria = "Al día", SinLocalFisico = true },
        new() { Id = 8, IdPredial = "SIN-LOCAL-02", NumeroFinca = "No aplica", Direccion = "Cobertura cantonal sin establecimiento permanente", Distrito = "Zapote", Propietario = "No aplica", CondicionOcupacion = "No aplica", AreaLocal = 0, CuentaServiciosMunicipales = "No aplica", NombreLocal = "Consultoría Integral", EstadoGis = "Referencial", EstadoCuentaTributaria = "Pendiente validación", SinLocalFisico = true },
        new() { Id = 9, IdPredial = "SJ-870011", NumeroFinca = "1-098811", Direccion = "Centro comercial sur, local 19", Distrito = "Mata Redonda", Propietario = "Eventos Metropolitanos S.A.", CondicionOcupacion = "Arrendatario", AreaLocal = 85, CuentaServiciosMunicipales = "CSM-24009", NombreLocal = "Eventos Metro", EstadoGis = "Validado", EstadoCuentaTributaria = "Al día" },
        new() { Id = 10, IdPredial = "SJ-440022", NumeroFinca = "1-090022", Direccion = "Zona industrial oeste, bodega 12", Distrito = "Uruca", Propietario = "Logística del Valle S.A.", CondicionOcupacion = "Propietario", AreaLocal = 310, CuentaServiciosMunicipales = "CSM-24010", NombreLocal = "Centro Logístico del Valle", EstadoGis = "Validado", EstadoCuentaTributaria = "Al día" }
    };

    private static List<ActividadEconomicaDto> BuildActivities() => new()
    {
        new() { CodigoCaecr = "4741", Descripcion = "Venta de equipos y accesorios tecnológicos", Categoria = "Comercial", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "5611", Descripcion = "Restaurantes y sodas", Categoria = "Servicios", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "4752", Descripcion = "Ferreterías y materiales de construcción", Categoria = "Industrial", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "5630", Descripcion = "Expendio de bebidas alcohólicas", Categoria = "Licores", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "8230", Descripcion = "Organización de eventos temporales", Categoria = "Temporal", RequiereUsoSuelo = false },
        new() { CodigoCaecr = "4789", Descripcion = "Comercio ambulante autorizado", Categoria = "Ambulante", RequiereUsoSuelo = false },
        new() { CodigoCaecr = "4922", Descripcion = "Transporte privado referencial", Categoria = "Transporte", RequiereUsoSuelo = false },
        new() { CodigoCaecr = "7310", Descripcion = "Publicidad exterior referencial", Categoria = "Publicidad", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "9000", Descripcion = "Espectáculos públicos", Categoria = "Temporal", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "0899", Descripcion = "Extracción de materiales referencial", Categoria = "Extractiva", RequiereUsoSuelo = true }
    };

    private static List<UsoSueloVinculadoDto> BuildLandUse(List<LicenciaComercialDto> licencias)
    {
        var result = licencias.Select(x => CloneLandUse(x.UsoSuelo)).ToList();
        while (result.Count < 10)
        {
            var index = result.Count + 1;
            result.Add(new UsoSueloVinculadoDto
            {
                NumeroCertificado = $"US-MOCK-{DateTime.Today.Year}-{index:000}",
                Estado = index % 3 == 0 ? "Pendiente" : index % 4 == 0 ? "No conforme" : "Conforme",
                EsConforme = index % 3 != 0 && index % 4 != 0,
                FechaValidacion = DateTime.Today.AddDays(-index * 4),
                Observaciones = index % 3 == 0 ? "Pendiente revisión técnica." : "Validación simulada para demo.",
                Fuente = "Uso de Suelo"
            });
        }

        return result.Take(10).ToList();
    }

    private static List<PatComplianceRecordDto> BuildCompliance(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatComplianceRecordDto
    {
        LicenciaId = x.Id,
        NumeroLicencia = x.NumeroLicencia,
        FechaInicioActividad = x.FechaSolicitud.AddDays(-15),
        TipoRegimen = index % 2 == 0 ? "Tradicional" : "Simplificado",
        TipoPeriodoFiscal = index % 3 == 0 ? "Especial" : "Ordinario",
        ActividadesEnOtrosCantones = index % 4 == 0,
        EstadoHacienda = index % 5 == 0 ? "Observado" : "Completo",
        EstadoCcss = index % 4 == 0 ? "Pendiente" : "Completo",
        EstadoFodesaf = index % 3 == 0 ? "Pendiente" : "Completo",
        EstadoIns = index % 2 == 0 ? "Completo" : "Pendiente",
        EstadoRevision = index % 5 == 0 ? "Observado" : index % 2 == 0 ? "Completo" : "Pendiente"
    }).ToList();

    private static List<PatMorosityValidationDto> BuildMorosity(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatMorosityValidationDto
    {
        LicenciaId = x.Id,
        NumeroLicencia = x.NumeroLicencia,
        SolicitanteAlDia = index % 4 != 0,
        PropietarioAlDia = index % 5 != 0,
        TieneArregloPago = index % 4 == 0,
        ObligacionesFormalesAlDia = index % 3 != 0,
        Resultado = index % 4 == 0 ? "Con arreglo" : index % 5 == 0 ? "Pendiente validación" : index % 3 == 0 ? "Moroso" : "Al día",
        Observaciones = index % 3 == 0 ? "Revisión mock de cobro municipal pendiente." : "Validación simulada satisfactoria."
    }).ToList();

    private static List<PatAssessmentDto> BuildAssessments(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatAssessmentDto
    {
        LicenciaId = x.Id,
        NumeroLicencia = x.NumeroLicencia,
        TipoTasacion = (index % 5) switch
        {
            0 => "Analogía",
            1 => "Declaración",
            2 => "Temporal",
            3 => "Licores",
            _ => "Transporte público"
        },
        MontoAnual = 420000 + index * 35000,
        MontoTrimestral = 105000 + index * 8750,
        TimbreBiodiversidadAnual = 18000 + index * 500,
        PublicidadExteriorAnual = index % 2 == 0 ? 12000 : 0,
        Multa = index % 3 == 0 ? 15000 : 0,
        Intereses = index % 4 == 0 ? 8500 : 0,
        FechaInicioCobro = DateTime.Today.AddDays(-(index + 1) * 20),
        Estado = index % 3 == 0 ? "Pendiente" : index % 2 == 0 ? "Aprobada" : "Calculada"
    }).ToList();

    private static List<PatResolutionDto> BuildResolutions(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatResolutionDto
    {
        LicenciaId = x.Id,
        NumeroLicencia = x.NumeroLicencia,
        NumeroResolucion = $"RES-PAT-{DateTime.Today.Year}-{index + 1:000}",
        Estado = index % 3 == 0 ? "Pendiente validación" : index % 4 == 0 ? "Prevenida" : "Aprobada",
        Resultado = index % 5 == 0 ? "Pendiente firma" : index % 4 == 0 ? "Prevenida" : "Aprobada",
        FechaResolucion = DateTime.Today.AddDays(-(index + 1) * 6),
        Firmante = index % 5 == 0 ? "Pendiente" : "Jefatura Patentes",
        NotificacionSimulada = index % 5 != 0,
        Observaciones = index % 4 == 0 ? "Uso de suelo o requisitos requieren revisión." : "Resolución simulada para demo MVP."
    }).ToList();

    private static List<PatReceivableDto> BuildReceivables(List<LicenciaComercialDto> licencias) => licencias.Take(10).Select((x, index) => new PatReceivableDto
    {
        NumeroContribuyente = x.Identificacion,
        NumeroLicencia = x.NumeroLicencia,
        Trimestre = $"T{(index % 4) + 1}",
        PeriodoFiscal = DateTime.Today.Year.ToString(),
        Subtributo = index % 2 == 0 ? "Patente Comercial" : "Timbre Biodiversidad",
        MontoImpuesto = 85000 + index * 4500,
        MontoTimbre = 4500 + index * 150,
        MontoPublicidadExterior = index % 3 == 0 ? 2500 : 0,
        FechaInicio = DateTime.Today.AddDays(-(index + 1) * 10),
        FechaVencimiento = DateTime.Today.AddDays((index + 1) * 15),
        Estado = index % 4 == 0 ? "Pendiente" : index % 5 == 0 ? "Exonerado" : "Generada"
    }).ToList();

    private static List<PatPaymentDto> BuildPayments(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatPaymentDto
    {
        NumeroLicencia = x.NumeroLicencia,
        Identificacion = x.Identificacion,
        NombreContribuyente = x.ContribuyenteNombre,
        EntidadRecaudadora = index % 2 == 0 ? "Caja Municipal" : "Convenio Bancario",
        NumeroFactura = $"FAC-PAT-{DateTime.Today.Year}-{index + 1:0000}",
        FechaPago = DateTime.Today.AddDays(-(index + 1) * 8),
        Trimestre = $"T{(index % 4) + 1}",
        PeriodoFiscal = DateTime.Today.Year.ToString(),
        MontoImpuestoCancelado = 76000 + index * 3000,
        MontoTimbre = 4200 + index * 100,
        MontoPublicidadExterior = index % 3 == 0 ? 2000 : 0,
        MontoIntereses = index % 4 == 0 ? 850 : 0,
        MontoMultas = index % 5 == 0 ? 1200 : 0,
        Estado = index % 5 == 0 ? "Aplicado con observación" : "Registrado"
    }).ToList();

    private static List<PatTaxAccountEntryDto> BuildTaxAccount(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatTaxAccountEntryDto
    {
        NumeroLicencia = x.NumeroLicencia,
        Identificacion = x.Identificacion,
        Trimestre = $"T{(index % 4) + 1}",
        PeriodoFiscal = DateTime.Today.Year.ToString(),
        CodigoTributario = $"TRB-{120 + index}",
        Descripcion = index % 2 == 0 ? "Patente comercial trimestral" : "Timbre biodiversidad",
        CodigoSubtributo = index % 2 == 0 ? "PAT-COM" : "TIM-BIO",
        TipoMovimiento = index % 3 == 0 ? "Débito" : "Crédito mock",
        Monto = 82000 + index * 2800,
        Estado = index % 4 == 0 ? "Pendiente" : "Aplicado",
        FechaCreacion = DateTime.Today.AddDays(-(index + 1) * 12)
    }).ToList();

    private static List<PatDeclarationDto> BuildDeclarations(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatDeclarationDto
    {
        NumeroDeclaracion = $"DEC-PAT-{DateTime.Today.Year}-{index + 1:000}",
        NumeroLicencia = x.NumeroLicencia,
        FechaPresentacion = DateTime.Today.AddDays(-(index + 1) * 18),
        Canal = (index % 3) switch
        {
            0 => "Presencial",
            1 => "MIMUNIENCASA",
            _ => "VUI"
        },
        PeriodoFiscal = (DateTime.Today.Year - (index % 2)).ToString(),
        TipoDeclaracion = (index % 4) switch
        {
            0 => "Patente comercial",
            1 => "Rectificación",
            2 => "Licores",
            _ => "Espectáculos públicos"
        },
        Estado = index % 5 == 0 ? "Vencida" : index % 3 == 0 ? "Pendiente" : "Aprobada",
        IngresosBrutos = 2500000 + index * 175000,
        Gastos = 1100000 + index * 90000,
        Deducciones = 180000 + index * 7500,
        MontoImpuesto = 98000 + index * 4500,
        TimbreBiodiversidad = 4200 + index * 110,
        Multa = index % 4 == 0 ? 8500 : 0
    }).ToList();

    private static List<PatExemptionDto> BuildExemptions(List<LicenciaComercialDto> licencias) => licencias.Take(5).Select((x, index) => new PatExemptionDto
    {
        NumeroLicencia = x.NumeroLicencia,
        TipoExoneracion = (index % 4) switch
        {
            0 => "Ideas Productivas",
            1 => "Zona Franca",
            2 => "Bien social",
            _ => "Otro"
        },
        Origen = index % 2 == 0 ? "Patentes" : "Concejo Municipal",
        OrganizacionBeneficiaria = index % 2 == 0 ? x.ContribuyenteNombre : "Asociación de Desarrollo Distrital",
        EsTotal = index % 3 == 0,
        Porcentaje = index % 3 == 0 ? 100 : 50,
        MontoExonerado = 45000 + index * 5000,
        FechaInicio = DateTime.Today.AddMonths(-(index + 1)),
        FechaFin = DateTime.Today.AddMonths(index + 2),
        Estado = index % 4 == 0 ? "Activa" : index % 3 == 0 ? "Vencida" : "Aprobada"
    }).ToList();

    private static List<PatEmissionDto> BuildEmissions() => new()
    {
        new() { TipoEmision = "Patentes Comerciales", PeriodoFiscal = DateTime.Today.Year.ToString(), PeriodoEmision = "I Trimestre", Estado = "Emitida mock", CantidadLicencias = 10, MontoTotal = 1285000, TimbreBiodiversidad = 48000, Multas = 18000, PublicidadExterior = 12000, Inconsistencias = 1, ControlCalidad = "Aprobado" },
        new() { TipoEmision = "Patentes Comerciales", PeriodoFiscal = DateTime.Today.Year.ToString(), PeriodoEmision = "II Trimestre", Estado = "En control de calidad", CantidadLicencias = 9, MontoTotal = 1172000, TimbreBiodiversidad = 45200, Multas = 9500, PublicidadExterior = 9800, Inconsistencias = 2, ControlCalidad = "Pendiente" },
        new() { TipoEmision = "Patentes Comerciales", PeriodoFiscal = (DateTime.Today.Year - 1).ToString(), PeriodoEmision = "IV Trimestre", Estado = "Aprobada", CantidadLicencias = 8, MontoTotal = 1096000, TimbreBiodiversidad = 43000, Multas = 14000, PublicidadExterior = 8600, Inconsistencias = 0, ControlCalidad = "Aprobado" },
        new() { TipoEmision = "Licores", PeriodoFiscal = DateTime.Today.Year.ToString(), PeriodoEmision = "I Trimestre", Estado = "Emitida mock", CantidadLicencias = 4, MontoTotal = 842000, TimbreBiodiversidad = 28000, Multas = 9000, PublicidadExterior = 0, Inconsistencias = 1, ControlCalidad = "Aprobado" },
        new() { TipoEmision = "Licores", PeriodoFiscal = (DateTime.Today.Year - 1).ToString(), PeriodoEmision = "IV Trimestre", Estado = "Aprobada", CantidadLicencias = 4, MontoTotal = 790000, TimbreBiodiversidad = 26500, Multas = 6000, PublicidadExterior = 0, Inconsistencias = 0, ControlCalidad = "Aprobado" }
    };

    private static List<PatTemporaryLicenseDto> BuildTemporaryLicenses() => new()
    {
        new() { NumeroSolicitud = "TEMP-2025-001", TipoEvento = "Feria", FechaInicio = DateTime.Today.AddDays(7), FechaFin = DateTime.Today.AddDays(10), Lugar = "Parque central", Actividad = "Feria gastronómica", Responsable = "Eventos Metropolitanos S.A.", IncluyeBebidasAlcoholicas = false, ViaPublica = true, EstadoResolucion = "En revisión", CobroUnico = 125000 },
        new() { NumeroSolicitud = "TEMP-2025-002", TipoEvento = "Fiesta patronal", FechaInicio = DateTime.Today.AddDays(20), FechaFin = DateTime.Today.AddDays(22), Lugar = "Salón comunal", Actividad = "Venta de comidas", Responsable = "Asociación de Desarrollo", IncluyeBebidasAlcoholicas = false, ViaPublica = false, EstadoResolucion = "Aprobada", CobroUnico = 98000 },
        new() { NumeroSolicitud = "TEMP-2025-003", TipoEvento = "Puesto navideño", FechaInicio = DateTime.Today.AddDays(40), FechaFin = DateTime.Today.AddDays(75), Lugar = "Boulevard sur", Actividad = "Venta estacional", Responsable = "Karen Brenes Alpízar", IncluyeBebidasAlcoholicas = false, ViaPublica = true, EstadoResolucion = "Pendiente validación", CobroUnico = 155000 },
        new() { NumeroSolicitud = "TEMP-2025-004", TipoEvento = "Espectáculo público", FechaInicio = DateTime.Today.AddDays(12), FechaFin = DateTime.Today.AddDays(13), Lugar = "Estadio municipal", Actividad = "Concierto referencial", Responsable = "Eventos Metropolitanos S.A.", IncluyeBebidasAlcoholicas = true, ViaPublica = false, EstadoResolucion = "Prevenida", CobroUnico = 230000 }
    };

    private static List<PatLiquorLicenseDto> BuildLiquorLicenses(List<LicenciaComercialDto> licencias)
    {
        var reference = licencias.Take(4).ToList();
        return new List<PatLiquorLicenseDto>
        {
            new() { NumeroLicenciaLicores = "LIC-2025-001", NumeroLicenciaComercial = reference[0].NumeroLicencia, NumeroExpediente = reference[0].Solicitud.NumeroSolicitud, NombreComercial = "TecnoCentro Lounge", Patentado = reference[0].ContribuyenteNombre, Categoria = "C", Subcategoria = "Restaurante con licor", Distrito = "San Francisco", FechaAprobacion = DateTime.Today.AddMonths(-4), FechaVencimiento = DateTime.Today.AddYears(5).AddMonths(-4), EstadoRenovacion = "Vigente", EstadoVigencia = "Activa" },
            new() { NumeroLicenciaLicores = "LIC-2024-018", NumeroLicenciaComercial = reference[1].NumeroLicencia, NumeroExpediente = reference[1].Solicitud.NumeroSolicitud, NombreComercial = "Cafe Central Bistro", Patentado = reference[1].ContribuyenteNombre, Categoria = "D", Subcategoria = "Bar y restaurante", Distrito = "Carmen", FechaAprobacion = DateTime.Today.AddMonths(-10), FechaVencimiento = DateTime.Today.AddYears(5).AddMonths(-10), EstadoRenovacion = "Pendiente", EstadoVigencia = "Activa" },
            new() { NumeroLicenciaLicores = "LIC-2023-009", NumeroLicenciaComercial = reference[2].NumeroLicencia, NumeroExpediente = reference[2].Solicitud.NumeroSolicitud, NombreComercial = "Ferretería Hospital Bar", Patentado = reference[2].ContribuyenteNombre, Categoria = "A", Subcategoria = "Licorería", Distrito = "Hospital", FechaAprobacion = DateTime.Today.AddYears(-2), FechaVencimiento = DateTime.Today.AddYears(3), EstadoRenovacion = "Requiere revisión", EstadoVigencia = "Activa" },
            new() { NumeroLicenciaLicores = "LIC-2025-021", NumeroLicenciaComercial = reference[3].NumeroLicencia, NumeroExpediente = reference[3].Solicitud.NumeroSolicitud, NombreComercial = "Bar Mirador", Patentado = reference[3].ContribuyenteNombre, Categoria = "E", Subcategoria = "Temporal con licor", Distrito = "Carmen", FechaAprobacion = DateTime.Today.AddMonths(-2), FechaVencimiento = DateTime.Today.AddYears(5).AddMonths(-2), EstadoRenovacion = "Vigente", EstadoVigencia = "Suspendida referencial" }
        };
    }

    private static List<PatShareCapitalDeclarationDto> BuildShareCapital() => new()
    {
        new() { NumeroDeclaracion = "CAP-2025-001", NumeroLicencia = "LIC-2025-001", TipoDeclaracion = "Capital accionario", FechaPresentacion = DateTime.Today.AddDays(-40), Estado = "Registrada", VigenciaHasta = DateTime.Today.AddYears(1), AlertaDebidoProceso = false, TotalAcciones = 1000 },
        new() { NumeroDeclaracion = "CAP-2025-002", NumeroLicencia = "LIC-2024-018", TipoDeclaracion = "Capital accionario", FechaPresentacion = DateTime.Today.AddDays(-65), Estado = "Aprobada", VigenciaHasta = DateTime.Today.AddMonths(10), AlertaDebidoProceso = true, TotalAcciones = 2000 },
        new() { NumeroDeclaracion = "CAP-2024-009", NumeroLicencia = "LIC-2023-009", TipoDeclaracion = "Capital accionario", FechaPresentacion = DateTime.Today.AddMonths(-6), Estado = "Pendiente", VigenciaHasta = DateTime.Today.AddMonths(6), AlertaDebidoProceso = false, TotalAcciones = 750 },
        new() { NumeroDeclaracion = "CAP-2025-004", NumeroLicencia = "LIC-2025-021", TipoDeclaracion = "Capital accionario", FechaPresentacion = DateTime.Today.AddDays(-15), Estado = "Registrada", VigenciaHasta = DateTime.Today.AddYears(1), AlertaDebidoProceso = false, TotalAcciones = 500 }
    };

    private static List<PatInspectionDto> BuildInspections(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatInspectionDto
    {
        NumeroInspeccion = $"INSP-PAT-{DateTime.Today.Year}-{index + 1:000}",
        NumeroExpediente = x.Solicitud.NumeroSolicitud,
        NumeroLicencia = x.NumeroLicencia,
        Origen = (index % 4) switch
        {
            0 => "Solicitud",
            1 => "Denuncia",
            2 => "Renovación",
            _ => "Fiscalización"
        },
        FechaSolicitud = DateTime.Today.AddDays(-(index + 1) * 7),
        FechaInspeccion = DateTime.Today.AddDays(-(index + 1) * 5),
        InspectorAsignado = index % 2 == 0 ? "Carlos Jiménez" : "Rebeca Ureña",
        Estado = index % 3 == 0 ? "En revisión" : index % 4 == 0 ? "Asignada" : "Finalizada",
        InspeccionEfectiva = index % 5 != 0,
        Resultado = index % 4 == 0 ? "Prevención" : index % 5 == 0 ? "Infracción" : "Conforme"
    }).ToList();

    private static List<PatInfractionDto> BuildInfractions() => new()
    {
        new() { NumeroGestion = "INF-2025-001", NumeroLicencia = "PAT-2025-018", NumeroInspeccion = "INSP-PAT-2025-002", TipoInfraccion = "Incumplimiento de horarios", AplicaMulta = true, RequiereProcesoAdministrativo = false, MontoMulta = 25000, Estado = "Registrada", Origen = "Inspección" },
        new() { NumeroGestion = "INF-2025-002", NumeroLicencia = "PAT-2025-009", NumeroInspeccion = "INSP-PAT-2025-004", TipoInfraccion = "Actividad no autorizada", AplicaMulta = true, RequiereProcesoAdministrativo = true, MontoMulta = 55000, Estado = "En análisis", Origen = "Denuncia" },
        new() { NumeroGestion = "INF-2025-003", NumeroLicencia = "PAT-2024-055", NumeroInspeccion = "INSP-PAT-2025-005", TipoInfraccion = "Incumplimiento de requisitos", AplicaMulta = false, RequiereProcesoAdministrativo = true, MontoMulta = 0, Estado = "Enviado a proceso", Origen = "Fiscalización" },
        new() { NumeroGestion = "INF-2025-004", NumeroLicencia = "PAT-2025-031", NumeroInspeccion = "INSP-PAT-2025-006", TipoInfraccion = "Publicidad exterior sin licencia", AplicaMulta = true, RequiereProcesoAdministrativo = false, MontoMulta = 18000, Estado = "Notificada", Origen = "Operativo" },
        new() { NumeroGestion = "INF-2025-005", NumeroLicencia = "PAT-2025-041", NumeroInspeccion = "INSP-PAT-2025-007", TipoInfraccion = "No cuenta con declaración", AplicaMulta = true, RequiereProcesoAdministrativo = true, MontoMulta = 32000, Estado = "Pendiente validación", Origen = "Control de calidad" }
    };

    private static List<PatAdministrativeProcessDto> BuildAdministrativeProcesses() => new()
    {
        new() { NumeroOrganoDirector = "PA-2025-001", NumeroLicencia = "PAT-2025-009", FechaEmision = DateTime.Today.AddDays(-30), FechaAudiencia = DateTime.Today.AddDays(10), Funcionario = "Jefatura Patentes", Estado = "Notificado", Resolucion = "Pendiente", AlertaVencimiento = false },
        new() { NumeroOrganoDirector = "PA-2025-002", NumeroLicencia = "PAT-2024-055", FechaEmision = DateTime.Today.AddDays(-45), FechaAudiencia = DateTime.Today.AddDays(-5), Funcionario = "Asesoría Legal", Estado = "En resolución", Resolucion = "Proyecto referencial", AlertaVencimiento = true },
        new() { NumeroOrganoDirector = "PA-2025-003", NumeroLicencia = "PAT-2025-041", FechaEmision = DateTime.Today.AddDays(-12), FechaAudiencia = DateTime.Today.AddDays(18), Funcionario = "Órgano Director", Estado = "Registrado", Resolucion = "Pendiente notificación", AlertaVencimiento = false }
    };

    private static List<PatComplaintDto> BuildComplaints(List<LicenciaComercialDto> licencias)
    {
        var selected = licencias.Take(6).ToList();
        return selected.Select((x, index) => new PatComplaintDto
        {
            NumeroExpediente = $"DEN-PAT-{DateTime.Today.Year}-{index + 1:000}",
            CanalIngreso = (index % 4) switch
            {
                0 => "Presencial",
                1 => "Plataforma de Servicios",
                2 => "MIMUNIENCASA",
                _ => "VUI"
            },
            Anonima = index % 3 == 0,
            TipoDenunciante = index % 3 == 0 ? "Anónimo" : "Ciudadanía",
            Motivo = index % 2 == 0 ? "Ruidos y horario" : "Actividad no autorizada",
            Estado = index % 4 == 0 ? "En inspección" : index % 5 == 0 ? "Archivada" : "En revisión",
            NumeroLicencia = x.NumeroLicencia,
            NombreComercial = x.NombreComercial,
            InspectorAsignado = index % 2 == 0 ? "Carlos Jiménez" : "Rebeca Ureña",
            Resultado = index % 4 == 0 ? "Inspección" : index % 5 == 0 ? "Archivo" : "Prevención"
        }).ToList();
    }

    private static List<PatNotificationDto> BuildNotifications(List<LicenciaComercialDto> licencias) => licencias.Take(8).Select((x, index) => new PatNotificationDto
    {
        NumeroLicencia = x.NumeroLicencia,
        Tipo = (index % 5) switch
        {
            0 => "Prevención",
            1 => "Aprobación",
            2 => "Cobro",
            3 => "Denuncia",
            _ => "Renovación"
        },
        Medio = index % 3 == 0 ? "Correo" : index % 3 == 1 ? "SMS" : "Domicilio fiscal",
        Estado = index % 4 == 0 ? "Pendiente" : index % 5 == 0 ? "Fallida" : "Enviada",
        Fecha = DateTime.Today.AddDays(-(index + 1) * 3),
        Resultado = index % 4 == 0 ? "Esperando cola de salida" : "Evento registrado",
        Destinatario = x.ContribuyenteNombre
    }).ToList();

    private static List<PatHistoryEntryDto> BuildHistory(List<LicenciaComercialDto> licencias, List<MovimientoPatenteDto> movimientos)
    {
        var result = new List<PatHistoryEntryDto>();

        foreach (var licencia in licencias.Take(12))
        {
            var expediente = licencia.Solicitud.NumeroSolicitud;
            result.Add(new PatHistoryEntryDto
            {
                NumeroLicencia = licencia.NumeroLicencia,
                NumeroExpediente = expediente,
                CampoModificado = "Estado",
                ValorAnterior = "En revisión",
                ValorNuevo = licencia.Estado,
                Proceso = "Gestión Patentes",
                Usuario = licencia.Responsable,
                FechaHora = licencia.FechaSolicitud.AddDays(2),
                EstadoControlCalidad = licencia.Estado == "Aprobado" ? "Aprobado" : "Pendiente",
                Origen = "Manual"
            });
        }

        foreach (var movimiento in movimientos.Take(12))
        {
            result.Add(new PatHistoryEntryDto
            {
                NumeroLicencia = licencias.FirstOrDefault(x => x.Movimientos.Any(m => m.FechaHora == movimiento.FechaHora && m.TipoMovimiento == movimiento.TipoMovimiento))?.NumeroLicencia ?? "PAT-REF",
                NumeroExpediente = licencias.FirstOrDefault(x => x.Movimientos.Any(m => m.FechaHora == movimiento.FechaHora && m.TipoMovimiento == movimiento.TipoMovimiento))?.Solicitud.NumeroSolicitud ?? "EXP-REF",
                CampoModificado = movimiento.TipoMovimiento,
                ValorAnterior = "N/A",
                ValorNuevo = movimiento.EstadoResultante,
                Proceso = movimiento.Origen,
                Usuario = movimiento.Usuario,
                FechaHora = movimiento.FechaHora,
                EstadoControlCalidad = movimiento.EstadoResultante == "Aprobado" ? "Aprobado" : "Pendiente",
                Origen = movimiento.Origen
            });
        }

        return result.OrderByDescending(x => x.FechaHora).Take(16).ToList();
    }

    private static List<PatAuditEventDto> BuildAuditEvents(List<MovimientoPatenteDto> movimientos) => movimientos.Take(10).Select((x, index) => new PatAuditEventDto
    {
        Modulo = "Patentes",
        Accion = x.TipoMovimiento,
        Usuario = x.Usuario,
        FechaHora = x.FechaHora,
        Descripcion = x.Motivo,
        Referencia = $"PAT-AUD-{index + 1:000}",
        Origen = x.Origen,
        Severidad = index % 4 == 0 ? "Alta" : index % 3 == 0 ? "Media" : "Informativa"
    }).ToList();

    private static List<PatQualityCaseDto> BuildQualityCases() => new()
    {
        new() { CodigoCaso = "QC-PAT-001", Origen = "Solicitud", Referencia = "SOL-PAT-2025-018", Estado = "Pendiente", MotivoDevolucion = "Validar uso de suelo", UsuarioResponsable = "tcalidad", FuncionarioAsignado = "Mariela Vargas", FechaRegistro = DateTime.Today.AddDays(-5) },
        new() { CodigoCaso = "QC-PAT-002", Origen = "Renovación", Referencia = "PAT-2023-077", Estado = "Devuelto", MotivoDevolucion = "Completar requisitos", UsuarioResponsable = "rcalidad", FuncionarioAsignado = "Luis Arce", FechaRegistro = DateTime.Today.AddDays(-10) },
        new() { CodigoCaso = "QC-PAT-003", Origen = "Inspección", Referencia = "INSP-PAT-2025-005", Estado = "Aprobado", MotivoDevolucion = string.Empty, UsuarioResponsable = "tcalidad", FuncionarioAsignado = "Carlos Jiménez", FechaRegistro = DateTime.Today.AddDays(-3) },
        new() { CodigoCaso = "QC-PAT-004", Origen = "Declaración", Referencia = "DEC-PAT-2025-004", Estado = "Reasignado", MotivoDevolucion = "Revisar montos mock", UsuarioResponsable = "jcalidad", FuncionarioAsignado = "Analista Declaraciones", FechaRegistro = DateTime.Today.AddDays(-8) },
        new() { CodigoCaso = "QC-PAT-005", Origen = "Emisión", Referencia = "II Trimestre", Estado = "Pendiente", MotivoDevolucion = "Validar inconsistencias", UsuarioResponsable = "rcalidad", FuncionarioAsignado = "Jefatura Patentes", FechaRegistro = DateTime.Today.AddDays(-1) }
    };

    private static List<PatReportDto> BuildReports() => new()
    {
        new() { Nombre = "Estado de cuenta", Descripcion = "Resumen mock de cargos y pagos por licencia.", Categoria = "Cobro", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Nombre contribuyente", "Número licencia", "Período fiscal" }, UltimaGeneracion = DateTime.Today.AddDays(-1) },
        new() { Nombre = "Reporte para tributación", Descripcion = "Consolidado referencial de licencias y declaraciones.", Categoria = "Tributación", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Identificación", "Rango de fechas" }, UltimaGeneracion = DateTime.Today.AddDays(-2) },
        new() { Nombre = "Reporte de emisión", Descripcion = "Control mock de emisión comercial y licores.", Categoria = "Emisión", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Período", "Estado" }, UltimaGeneracion = DateTime.Today.AddDays(-3) },
        new() { Nombre = "Reporte de declaraciones", Descripcion = "Historial mock de declaraciones presentadas.", Categoria = "Declaraciones", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Número licencia", "Canal" }, UltimaGeneracion = DateTime.Today.AddDays(-4) },
        new() { Nombre = "Estado de cuenta unificado", Descripcion = "Vista mock consolidada por contribuyente.", Categoria = "Cobro", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Contribuyente", "Identificación" }, UltimaGeneracion = DateTime.Today.AddDays(-5) },
        new() { Nombre = "Constancia de estar al día", Descripcion = "Documento referencial de cumplimiento.", Categoria = "Constancias", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Número licencia", "Estado" }, UltimaGeneracion = DateTime.Today.AddDays(-1) },
        new() { Nombre = "Constancia de ser patentado", Descripcion = "Emisión mock de constancia institucional.", Categoria = "Constancias", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Identificación", "Nombre contribuyente" }, UltimaGeneracion = DateTime.Today.AddDays(-6) },
        new() { Nombre = "Repositorio de licencias comerciales", Descripcion = "Listado mock general del padrón de patentes.", Categoria = "Repositorio", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Distrito", "Tipo licencia", "Estado" }, UltimaGeneracion = DateTime.Today.AddDays(-1) },
        new() { Nombre = "Repositorio de inspecciones", Descripcion = "Seguimiento mock de fiscalización.", Categoria = "Fiscalización", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Inspector", "Resultado", "Rango de fechas" }, UltimaGeneracion = DateTime.Today.AddDays(-2) },
        new() { Nombre = "Licencias próximas a vencer", Descripcion = "Control mock de vencimientos y renovaciones.", Categoria = "Vencimientos", Estado = "Disponible mock", FiltrosPrincipales = new List<string> { "Fecha vencimiento", "Distrito" }, UltimaGeneracion = DateTime.Today }
    };

    private static List<PatCatalogItemDto> BuildCatalogs() => new()
    {
        new() { Categoria = "Tipos de licencia", Codigo = "ORD", Nombre = "Ordinaria", Descripcion = "Licencia comercial ordinaria." },
        new() { Categoria = "Tipos de licencia", Codigo = "PER", Nombre = "Permanente", Descripcion = "Licencia permanente." },
        new() { Categoria = "Tipos de licencia", Codigo = "TMP", Nombre = "Temporal", Descripcion = "Licencia temporal." },
        new() { Categoria = "Tipos de licencia", Codigo = "AMB", Nombre = "Ambulante", Descripcion = "Actividad sin local fijo." },
        new() { Categoria = "Canal", Codigo = "PRS", Nombre = "Presencial", Descripcion = "Ingreso por ventanilla." },
        new() { Categoria = "Canal", Codigo = "PDS", Nombre = "Plataforma de Servicios", Descripcion = "Origen integrado referencial." },
        new() { Categoria = "Canal", Codigo = "MMC", Nombre = "MIMUNIENCASA", Descripcion = "Canal digital municipal." },
        new() { Categoria = "Canal", Codigo = "VUI", Nombre = "VUI", Descripcion = "Ventanilla única de inversión." },
        new() { Categoria = "Régimen", Codigo = "REG-01", Nombre = "Tradicional", Descripcion = "Régimen tributario base." },
        new() { Categoria = "Régimen", Codigo = "REG-02", Nombre = "Simplificado", Descripcion = "Régimen simplificado." },
        new() { Categoria = "Período fiscal", Codigo = "PF-01", Nombre = "Ordinario", Descripcion = "Período fiscal ordinario." },
        new() { Categoria = "Período fiscal", Codigo = "PF-02", Nombre = "Especial", Descripcion = "Período fiscal especial." },
        new() { Categoria = "Tipo evento temporal", Codigo = "FER", Nombre = "Feria", Descripcion = "Evento temporal ferial." },
        new() { Categoria = "Tipo evento temporal", Codigo = "ESP", Nombre = "Espectáculo público", Descripcion = "Actividad temporal masiva." },
        new() { Categoria = "Parámetro", Codigo = "SAL-BASE", Nombre = "Salario base", Descripcion = "Parámetro mock", EsParametro = true, ValorReferencia = "462200" },
        new() { Categoria = "Parámetro", Codigo = "TIM-BIO", Nombre = "Timbre biodiversidad", Descripcion = "Porcentaje mock", EsParametro = true, ValorReferencia = "0.02" },
        new() { Categoria = "Parámetro", Codigo = "TAS-AN", Nombre = "Tasación por analogía", Descripcion = "Factor mock", EsParametro = true, ValorReferencia = "1.15" },
        new() { Categoria = "Inspectores", Codigo = "INSP-01", Nombre = "Carlos Jiménez", Descripcion = "Inspector Patentes" },
        new() { Categoria = "Inspectores", Codigo = "INSP-02", Nombre = "Rebeca Ureña", Descripcion = "Inspectora Patentes" },
        new() { Categoria = "Exoneraciones", Codigo = "EX-01", Nombre = "Ideas Productivas", Descripcion = "Exoneración referencial." }
    };

    private static List<PatIntegrationStatusDto> BuildIntegrations() => new()
    {
        new() { NombreIntegracion = "Plataforma de Servicios", ModuloRelacionado = "Solicitudes", Estado = "Operativo", UltimoEvento = "Ingreso de solicitud", AccionVisual = "Ver referencia", Descripcion = "Canal de recepción institucional." },
        new() { NombreIntegracion = "MIMUNIENCASA", ModuloRelacionado = "Declaraciones", Estado = "Operativo", UltimoEvento = "Presentación registrada", AccionVisual = "Ver referencia", Descripcion = "Portal ciudadano." },
        new() { NombreIntegracion = "VUI", ModuloRelacionado = "Solicitudes", Estado = "Preparado", UltimoEvento = "Sincronización pendiente", AccionVisual = "Ver referencia", Descripcion = "Integración con ventanilla única." },
        new() { NombreIntegracion = "RUC", ModuloRelacionado = "Contribuyentes", Estado = "Referencial", UltimoEvento = "Consulta mock de contribuyente", AccionVisual = "Ver vínculo", Descripcion = "Reutiliza datos mock del RUC." },
        new() { NombreIntegracion = "Bienes Inmuebles", ModuloRelacionado = "Locales", Estado = "Configurado", UltimoEvento = "Validación de finca", AccionVisual = "Ver vínculo", Descripcion = "Consulta de predios." },
        new() { NombreIntegracion = "Permisos de Construcción", ModuloRelacionado = "Publicidad Exterior", Estado = "No incluido en MVP", UltimoEvento = "Sin evento", AccionVisual = "Deshabilitado", Descripcion = "Referencia futura." },
        new() { NombreIntegracion = "Comercial", ModuloRelacionado = "Cuenta tributaria", Estado = "Preparado", UltimoEvento = "Mapeo de tributos mock", AccionVisual = "Ver referencia", Descripcion = "Acople funcional futuro." },
        new() { NombreIntegracion = "Cobro", ModuloRelacionado = "Morosidad", Estado = "Operativo", UltimoEvento = "Validación de deuda", AccionVisual = "Ver referencia", Descripcion = "Consulta operativa." },
        new() { NombreIntegracion = "Conectividad", ModuloRelacionado = "Notificaciones", Estado = "No incluido en MVP", UltimoEvento = "Sin envío real", AccionVisual = "Deshabilitado", Descripcion = "Servicio de conectividad no implementado." },
        new() { NombreIntegracion = "Tesorería", ModuloRelacionado = "Pagos", Estado = "Referencial", UltimoEvento = "Aplicación mock de pago", AccionVisual = "Ver referencia", Descripcion = "Sin recaudación real." },
        new() { NombreIntegracion = "Cajas", ModuloRelacionado = "Pagos", Estado = "Referencial", UltimoEvento = "Factura mock generada", AccionVisual = "Ver referencia", Descripcion = "Integración visual בלבד." },
        new() { NombreIntegracion = "Reportería", ModuloRelacionado = "Reportes", Estado = "Operativo", UltimoEvento = "Generación solicitada", AccionVisual = "Ver referencia", Descripcion = "Reportes comerciales." },
        new() { NombreIntegracion = "Cuenta Tributaria", ModuloRelacionado = "Cobro", Estado = "Operativo", UltimoEvento = "Movimiento aplicado", AccionVisual = "Ver referencia", Descripcion = "Cuenta tributaria municipal." },
        new() { NombreIntegracion = "GIS", ModuloRelacionado = "Uso de Suelo", Estado = "Referencial", UltimoEvento = "Validación geográfica pendiente", AccionVisual = "Ver referencia", Descripcion = "Sin consulta geoespacial real." },
        new() { NombreIntegracion = "Bitácoras / Históricos", ModuloRelacionado = "Auditoría", Estado = "Operativo", UltimoEvento = "Evento registrado", AccionVisual = "Ver referencia", Descripcion = "Historial interno del expediente." }
    };

    private static LicenciaComercialDto CloneLicencia(LicenciaComercialDto item) => new()
    {
        Id = item.Id,
        NumeroLicencia = item.NumeroLicencia,
        ContribuyenteId = item.ContribuyenteId,
        ContribuyenteRuc = item.ContribuyenteRuc,
        Identificacion = item.Identificacion,
        ContribuyenteNombre = item.ContribuyenteNombre,
        NombreComercial = item.NombreComercial,
        DireccionLocal = item.DireccionLocal,
        FincaOIdPredial = item.FincaOIdPredial,
        ActividadEconomica = item.ActividadEconomica,
        CodigoCaecr = item.CodigoCaecr,
        TipoPatente = item.TipoPatente,
        Distrito = item.Distrito,
        Estado = item.Estado,
        FechaSolicitud = item.FechaSolicitud,
        FechaAprobacion = item.FechaAprobacion,
        FechaVencimiento = item.FechaVencimiento,
        Responsable = item.Responsable,
        Observaciones = item.Observaciones,
        PendienteUsoSuelo = item.PendienteUsoSuelo,
        EmisionSemestralPendiente = item.EmisionSemestralPendiente,
        NotificacionPendiente = item.NotificacionPendiente,
        CobroSincronizado = item.CobroSincronizado,
        GisValidado = item.GisValidado,
        UsoSuelo = CloneLandUse(item.UsoSuelo),
        Solicitud = CloneSolicitud(item.Solicitud),
        Requisitos = item.Requisitos.Select(CloneRequirement).ToList(),
        Movimientos = item.Movimientos.Select(CloneMovement).ToList()
    };

    private static SolicitudPatenteDto CloneSolicitud(SolicitudPatenteDto item) => new()
    {
        Id = item.Id,
        LicenciaId = item.LicenciaId,
        NumeroSolicitud = item.NumeroSolicitud,
        ContribuyenteId = item.ContribuyenteId,
        ContribuyenteRuc = item.ContribuyenteRuc,
        Identificacion = item.Identificacion,
        ContribuyenteNombre = item.ContribuyenteNombre,
        NombreComercial = item.NombreComercial,
        DireccionLocal = item.DireccionLocal,
        FincaOIdPredial = item.FincaOIdPredial,
        ActividadEconomica = item.ActividadEconomica,
        CodigoCaecr = item.CodigoCaecr,
        TipoPatente = item.TipoPatente,
        NumeroCertificadoUsoSuelo = item.NumeroCertificadoUsoSuelo,
        EstadoUsoSuelo = item.EstadoUsoSuelo,
        UsoSueloConforme = item.UsoSueloConforme,
        TipoTasacion = item.TipoTasacion,
        EstadoTasacion = item.EstadoTasacion,
        MontoAnualMock = item.MontoAnualMock,
        MontoTrimestralMock = item.MontoTrimestralMock,
        TimbreBiodiversidad = item.TimbreBiodiversidad,
        PublicidadExterior = item.PublicidadExterior,
        Multa = item.Multa,
        Intereses = item.Intereses,
        FechaInicioCobro = item.FechaInicioCobro,
        CobroProporcionalVisual = item.CobroProporcionalVisual,
        EstadoCobro = item.EstadoCobro,
        EstadoCuentaTributariaMock = item.EstadoCuentaTributariaMock,
        ReferenciaCuentaPorCobrarMock = item.ReferenciaCuentaPorCobrarMock,
        EstadoResolucion = item.EstadoResolucion,
        ObservacionesResolucion = item.ObservacionesResolucion,
        NotificacionSimulada = item.NotificacionSimulada,
        FirmaDigitalReferencial = item.FirmaDigitalReferencial,
        EstadoFinalExpediente = item.EstadoFinalExpediente,
        FechaSolicitud = item.FechaSolicitud,
        Responsable = item.Responsable,
        Estado = item.Estado,
        Observaciones = item.Observaciones,
        Adjuntos = item.Adjuntos.ToList(),
        Requisitos = item.Requisitos.Select(CloneRequirement).ToList(),
        UsoSuelo = CloneLandUse(item.UsoSuelo),
        Distrito = item.Distrito
    };

    private static UsoSueloVinculadoDto CloneLandUse(UsoSueloVinculadoDto item) => new()
    {
        NumeroCertificado = item.NumeroCertificado,
        Estado = item.Estado,
        EsConforme = item.EsConforme,
        FechaValidacion = item.FechaValidacion,
        Observaciones = item.Observaciones,
        Fuente = item.Fuente
    };

    private static RequisitoPatenteDto CloneRequirement(RequisitoPatenteDto item) => new()
    {
        Clave = item.Clave,
        Nombre = item.Nombre,
        Obligatorio = item.Obligatorio,
        Cumplido = item.Cumplido,
        Estado = item.Estado,
        Observacion = item.Observacion,
        DocumentoPlaceholder = item.DocumentoPlaceholder,
        Origen = item.Origen
    };

    private static MovimientoPatenteDto CloneMovement(MovimientoPatenteDto item) => new()
    {
        FechaHora = item.FechaHora,
        TipoMovimiento = item.TipoMovimiento,
        Motivo = item.Motivo,
        Usuario = item.Usuario,
        EstadoResultante = item.EstadoResultante,
        Origen = item.Origen
    };
}
