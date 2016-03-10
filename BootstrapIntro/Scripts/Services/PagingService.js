function PagingService(resultList) {
    var self = this;

    self.queryOptions = {
        currentPage: ko.observable(),
        totalPages: ko.observable(),
        pageSize: ko.observable(),
        sortField: ko.observable(),
        sortOrder: ko.observable()
    };
    
    self.entities = ko.observableArray();

    self.updateResultList = function (resultList) {
        self.queryOptions.currentPage(resultList.queryOptions.CurrentPage);
        self.queryOptions.totalPages(resultList.queryOptions.TotalPages);
        self.queryOptions.pageSize(resultList.queryOptions.PageSize);
        self.queryOptions.sortField(resultList.queryOptions.SortField);
        self.queryOptions.sortOrder(resultList.queryOptions.SortOrder);

        self.entities(resultList.results);
    };

    self.updateResultList(resultList);

    

    self.sortEntitiesBy = function (data, event) {
        var sortField = $(event.target).data('sortField');

        if (sortField == self.queryOptions.sortField() &&
             self.queryOptions.sortOrder() == "ASC")
            self.queryOptions.sortOrder("DESC");
        else
            self.queryOptions.sortOrder("ASC");
        self.queryOptions.sortField(sortField);
        self.queryOptions.currentPage(1);
        self.fetchEntities(event);
    };

    self.previousPage = function (data, event) {
        if (self.queryOptions.currentPage() > 1) {
            self.queryOptions.currentPage(self.queryOptions.currentPage() - 1);
            self.fetchEntities(event);
        }
    };

    self.nextPage = function (data, event) {
        if (self.queryOptions.currentPage() < self.queryOptions.totalPages()) {
            self.queryOptions.currentPage(self.queryOptions.currentPage() + 1);
            self.fetchEntities(event);
        }
    };

    self.fetchEntities = function (event) {
        var url = '/api/' + $(event.target).attr('href');
        url += "?sortField=" + self.queryOptions.sortField();
        url += "&sortOrder=" + self.queryOptions.sortOrder();
        url += "&currentPage=" + self.queryOptions.currentPage();
        url += "&pageSize=" + self.queryOptions.pageSize();
        $.ajax({
            dataType: 'json',
            url: url
        }).success(function (data) {
            self.updateResultList(data);
        }).error(function () {
            $('.body-content').prepend('<div class="alert alert-danger"><strong>Error!</strong> There was an error fetching the data.</div>');
        });
    };

    self.buildSortIcon = function (sortField) {
        return ko.pureComputed(function () {
            var sortIcon = 'sort';
            if (self.queryOptions.sortField() == sortField) {
                sortIcon += '-by-alphabet';
                if (self.queryOptions.sortOrder() == "DESC")
                    sortIcon += '-alt';
            }
            return 'glyphicon glyphicon-' + sortIcon;
        });
    };

    self.buildPreviousClass = ko.pureComputed(function () {
        var className = 'previous';
        if (self.queryOptions.currentPage() == 1)
            className += ' disabled';
        return className;
    });

    self.buildNextClass = ko.pureComputed(function () {
        var className = 'next';
        if (self.queryOptions.currentPage() == self.queryOptions.totalPages())
            className += ' disabled';
        return className;
    });
}

