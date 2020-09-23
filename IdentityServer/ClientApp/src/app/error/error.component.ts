import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ErrorsApiService } from '../services/errors-api.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {
  public showErrorDetails: boolean = false;
  public error: string;
  public showErrorDescription: boolean = false;
  public errorDescription: string;
  public showRequestId: boolean = false;
  public requestId: string;


  constructor(private errors: ErrorsApiService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    var errorId = this.getErrorId();

    this.errors.GetErrorDetails(errorId)
      .subscribe((response: any) => {

        if (response.error) {
          this.error = response.error;
          this.showErrorDetails = true;
        }

        if (response.errorDescription) {
          this.showErrorDescription = true;
          this.errorDescription = response.errorDescription;
        }

        if (response.requestId) {
          this.showRequestId = true;
          this.requestId = response.requestId;
        }

      });
  }

  private getErrorId(): string {
    return this.route.snapshot.queryParams['errorId'];
  }

}
