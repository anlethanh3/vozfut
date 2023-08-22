import { useEffect } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown } from "react-bootstrap";
import moment from 'moment';
import SearchMember from "../components/SearchMember";
import { selectState, onChangePageIndex, onChangePageSize, fetchAsync } from '../slices/matchSlice';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { NavLink } from "react-router-dom";

export default function Match() {
    let sizes = [10, 50, 100]
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize }))
            .unwrap()
            .then(values => {

            })
            .catch(error => {

            })
    }

    useEffect(() => {
        console.log('effect', state)
        fetch()
        return function cleanup() {
            console.log('unmount', state)
        }

    }, [state.pageIndex, state.pageSize])
    return (
        <>
            <h1>Matches</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch({ type: "failure", error: undefined })} dismissible>
                    {state.error}
                </Alert>
            }

            <SearchMember onSearchChanged={() => { }} onSubmit={() => { }} />

            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <Button variant="primary" onClick={() => { }}>Add match</Button><div className="mx-2" />
                </Col>
            </Row>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Team count</th>
                        <th>Team size</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map(value =>
                            <tr key={`key-${value.id}`}>
                                <td>{value.id}</td>
                                <td>{value.name}</td>
                                <td>{value.description}</td>
                                <td>{value.teamCount}</td>
                                <td>{value.teamSize}</td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <NavLink to={`${value.id}`}>
                                        <Button variant="primary" className="me-2" onClick={() => { }}>Member</Button>
                                    </NavLink>
                                    <Button variant="warning" className="me-2" onClick={() => { }}>Edit</Button>
                                    <Button variant="danger" className="me-2" onClick={() => { }}>Delete</Button>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
            {
                state.totalPage &&
                <>
                    <Row>
                        <Col>
                            <DropdownButton className="d-inline" size="lg" variant="secondary" id="dropdown-basic-button" title={state.pageSize}>
                                {
                                    sizes.map(element =>
                                        <Dropdown.Item key={`ditem-${element}`} onClick={() => dispatch(onChangePageSize(element))}>{element}</Dropdown.Item>
                                    )
                                }
                            </DropdownButton> <p className="d-inline">Records Page {state.pageIndex + 1} of {state.totalPage}</p></Col>
                        <Col>
                            <Pagination className="d-flex justify-content-end">
                                {
                                    Array.from(Array(state.totalPage).keys()).map(element => {
                                        var pageNumber = element + 1
                                        return <Pagination.Item
                                            onClick={() => dispatch(onChangePageIndex(pageNumber - 1))}
                                            key={`pitem-${element}`}
                                            active={state.pageIndex === pageNumber - 1}>{pageNumber}</Pagination.Item>
                                    })
                                }
                            </Pagination>
                        </Col>
                    </Row>
                </>
            }

        </>
    )
}