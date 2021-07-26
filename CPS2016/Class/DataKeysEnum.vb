Public Class DataKeysEnum

    Enum ActivityID As Short
        IndigoDownload = 1
        Indigo
        Assembly
        Production
        QualityControl
        Perso
        Done
    End Enum

    Enum ModuleID As Short
        Home_CPS = 1
        CardControl
        User
        Role
        SystemModule
        UploadPO
        IndigoExtract
        PendingForProcessCard
        PendingForProcessSheet
        RejectCard
        Download
        QAGoodReject
        Search_Track
        Process_DataPurge
        LoggedUsers
        SystemParameter
        Home_Inventory
        Inventory
        Processed_Inv
        Material
        SystemLog
        ErrorLog
        POReports
        DeliveryReceipt
        MiscReports
    End Enum

    Enum Report As Short
        SummaryOfCreatedPrintOrderReport = 1 'A
        ListOfInvalidRecords
        SummaryOfInitialPrintAndReprintRequest 'C
        DeliveryReceipt
        ElectronicReportOfDeliveredCardsPerPrintOrder 'E
        ElectronicReportOfRejectedCardsForConfirmation
        ElectronicReportOfGoodCards 'G
        ReportOfConfirmedRejectedCardsForReprintToOtherPrintOrder
        DailyReportOfDeactivatedCards 'I
        MontlyReportOfCancelledCards '10
        ReportOfLostDamagedCards 'K
        SummaryReportOfCardsProcessedByBatchPrintOrder
        InspectionQualityControlAndYieldReport
        CustomerReturnsTagged
        InternalRejects
        CertificateOfDeletion
        MaterialInventory
        RejectReportDaily
        RejectReportByPO
        DeliveryReceipt2
        DeliveryReceipt_PDF
        CertificateOfDeletion_v3_manual
        EditNamesAddress
        AcknowledgeFile
        CardCarrier_UBP
        ResponseFile
    End Enum

    Enum RoleID As Short
        Viewer = 1
        Indigo
        Assembly
        QualityControl
        Personalization
        InventoryAdmin
        Admin
        SystemAdmin
        Supervisor
        InventorySpoiledEditor
        StatusChangerAdmin
    End Enum

End Class
