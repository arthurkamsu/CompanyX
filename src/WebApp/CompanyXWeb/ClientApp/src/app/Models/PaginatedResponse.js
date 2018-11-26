"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PaginatedResponse = /** @class */ (function () {
    function PaginatedResponse(pageIndex, count, total, employees) {
        this.pageIndex = pageIndex;
        this.count = count;
        this.total = total;
        this.employees = employees;
    }
    return PaginatedResponse;
}());
exports.PaginatedResponse = PaginatedResponse;
//# sourceMappingURL=PaginatedResponse.js.map